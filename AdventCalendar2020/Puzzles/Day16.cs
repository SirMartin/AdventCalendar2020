using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security;
using AdventCalendar2020.Interfaces;

namespace AdventCalendar2020.Puzzles
{
    public class Day16 : AdventCalendarDay
    {
        public override string DayNumber => "16";
        public override (string, string) ExpectedResult => ("27850", "491924517533");

        /// <summary>
        /// --- Day 16: Ticket Translation ---
        /// As you're walking to yet another connecting flight, you realize that one of the legs of your re-routed trip coming up is on a high-speed train. However, the train ticket you were given is in a language you don't understand.You should probably figure out what it says before you get to the train station after the next flight.
        ///
        /// Unfortunately, you can't actually read the words on the ticket. You can, however, read the numbers, and so you figure out the fields these tickets must have and the valid ranges for values in those fields.
        ///
        /// You collect the rules for ticket fields, the numbers on your ticket, and the numbers on other nearby tickets for the same train service (via the airport security cameras) together into a single document you can reference(your puzzle input).
        ///
        /// The rules for ticket fields specify a list of fields that exist somewhere on the ticket and the valid ranges of values for each field.For example, a rule like class: 1-3 or 5-7 means that one of the fields in every ticket is named class and can be any value in the ranges 1-3 or 5-7 (inclusive, such that 3 and 5 are both valid in this field, but 4 is not).
        ///
        /// Each ticket is represented by a single line of comma-separated values.The values are the numbers on the ticket in the order they appear; every ticket has the same format.For example, consider this ticket:
        ///
        /// .--------------------------------------------------------.
        /// | ????: 101    ?????: 102   ??????????: 103     ???: 104 |
        /// |                                                        |
        /// | ??: 301  ??: 302             ???????: 303      ??????? |
        /// | ??: 401  ??: 402           ???? ????: 403    ????????? |
        /// '--------------------------------------------------------'
        /// Here, ? represents text in a language you don't understand. This ticket might be represented as 101,102,103,104,301,302,303,401,402,403; of course, the actual train tickets you're looking at are much more complicated.In any case, you've extracted just the numbers in such a way that the first number is always the same specific field, the second number is always a different specific field, and so on - you just don't know what each position actually means!
        ///
        ///
        /// Start by determining which tickets are completely invalid; these are tickets that contain values which aren't valid for any field. Ignore your ticket for now.
        ///
        /// For example, suppose you have the following notes:
        ///
        /// class: 1-3 or 5-7
        /// row: 6-11 or 33-44
        /// seat: 13-40 or 45-50
        ///
        /// your ticket:
        /// 7,1,14
        ///
        /// nearby tickets:
        /// 7,3,47
        /// 40,4,50
        /// 55,2,20
        /// 38,6,12
        /// It doesn't matter which position corresponds to which field; you can identify invalid nearby tickets by considering only whether tickets contain values that are not valid for any field. In this example, the values on the first nearby ticket are all valid for at least one field. This is not true of the other three nearby tickets: the values 4, 55, and 12 are are not valid for any field. Adding together all of the invalid values produces your ticket scanning error rate: 4 + 55 + 12 = 71.
        ///
        /// Consider the validity of the nearby tickets you scanned.What is your ticket scanning error rate?
        /// </summary>
        internal override string RunPuzzle1()
        {
            var inputLines = GetInputLines();

            var ticketValidator = new TicketValidator(inputLines);

            var notValidValues = ticketValidator.ValuesNotValid();

            return notValidValues.Sum().ToString();
        }

        /// <summary>
        /// --- Part Two ---
        /// Now that you've identified which tickets contain invalid values, discard those tickets entirely. Use the remaining valid tickets to determine which field is which.
        /// 
        ///     Using the valid ranges for each field, determine what order the fields appear on the tickets.The order is consistent between all tickets: if seat is the third field, it is the third field on every ticket, including your ticket.
        /// 
        ///     For example, suppose you have the following notes:
        /// 
        /// class: 0-1 or 4-19
        /// row: 0-5 or 8-19
        /// seat: 0-13 or 16-19
        /// 
        /// your ticket:
        /// 11,12,13
        /// 
        /// nearby tickets:
        /// 3,9,18
        /// 15,1,5
        /// 5,14,9
        /// Based on the nearby tickets in the above example, the first position must be row, the second position must be class, and the third position must be seat; you can conclude that in your ticket, class is 12, row is 11, and seat is 13.
        /// 
        /// Once you work out which field is which, look for the six fields on your ticket that start with the word departure.What do you get if you multiply those six values together?
        /// </summary>
        internal override string RunPuzzle2()
        {
            var inputLines = GetInputLines();

            var ticketValidator = new TicketValidator(inputLines);

            ticketValidator.ValuesNotValid();

            var validRulePositions = ticketValidator.FindRulePositions().Where(x => x.Value.StartsWith("departure"));

            var result = 1L;
            foreach (var validRulePosition in validRulePositions)
            {
                result *= ticketValidator.MyTicket[validRulePosition.Key];
            }

            return result.ToString();
        }
    }

    public class TicketValidator
    {
        public Dictionary<string, string> Rules { get; }
        public List<int> MyTicket { get; }
        public List<List<int>> NearbyTickets { get; }
        public List<List<int>> ValidTickets { get; set; }

        public TicketValidator(string[] inputLines)
        {
            Rules = new Dictionary<string, string>();
            NearbyTickets = new List<List<int>>();
            ValidTickets = new List<List<int>>();

            var isMyTicket = false;
            var isOtherTickets = false;
            for (var i = 0; i < inputLines.Length; i++)
            {
                var line = inputLines[i];

                if (string.IsNullOrWhiteSpace(line))
                {
                    // Change type.
                    if (!isMyTicket)
                    {
                        isMyTicket = true;
                        i++;
                    }
                    else if (!isOtherTickets)
                    {
                        isOtherTickets = true;
                        i++;
                    }

                    continue;
                }

                if (!isMyTicket)
                {
                    // Rules
                    var name = line.Split(':')[0];
                    var rulesText = line.Substring(line.IndexOf(':') + 1).Trim();

                    Rules.Add(name, TranslateRules(rulesText));
                }
                else if (!isOtherTickets)
                {
                    // My ticket.
                    MyTicket = ParseTicket(line);
                }
                else
                {
                    // Nearby tickets.
                    NearbyTickets.Add(ParseTicket(line));
                }
            }
        }

        private string TranslateRules(string value)
        {
            // Take the different parts.
            var parts = value.Split(new[] { "or" }, StringSplitOptions.None).Select(x => x.Trim());

            var results = new List<string>();
            foreach (var part in parts)
            {
                var p = part.Split('-');
                var min = Convert.ToInt32(p[0]);
                var max = Convert.ToInt32(p[1]);
                results.Add(string.Join(",", Enumerable.Range(min, max - min + 1)));
            }


            return string.Join(",", results);
        }

        private List<int> ParseTicket(string line)
        {
            return line.Split(',').Select(x => Convert.ToInt32(x)).ToList();
        }

        public List<int> ValuesNotValid()
        {
            var result = new List<int>();

            foreach (var ticket in NearbyTickets)
            {
                var isValid = true;
                foreach (var t in ticket)
                {
                    if (!Rules.Any(x => x.Value.Split(',').Contains(t.ToString())))
                    {
                        result.Add(t);
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    ValidTickets.Add(ticket);
                }
            }

            return result;
        }

        // TODO: Optimize ir, it takes around 18 seconds.
        public Dictionary<int, string> FindRulePositions()
        {
            var rulePositions = new Dictionary<int, string>();
            var ticketsToCheck = new List<List<int>>(ValidTickets);
            ticketsToCheck.Add(MyTicket);

            while (Rules.Count != rulePositions.Count)
            {
                for (var ticketIndex = 0; ticketIndex < MyTicket.Count; ticketIndex++)
                {
                    List<string> possibleRules = null;
                    foreach (var ticket in ticketsToCheck)
                    {
                        var validRules = Rules.Where(x => x.Value.Split(',').Contains(ticket[ticketIndex].ToString()))
                            .Select(x => x.Key).ToList();

                        // Remove rules assigned.
                        validRules = validRules.Where(x => !rulePositions.ContainsValue(x)).ToList();

                        if (possibleRules == null)
                        {
                            // First ticket.
                            possibleRules = new List<string>(validRules);
                        }
                        else
                        {
                            // Rest of tickets.
                            var stillValidIndexes = possibleRules.Intersect(validRules);
                            possibleRules = new List<string>(stillValidIndexes);
                        }

                        if (possibleRules.Count == 1)
                        {
                            if (!rulePositions.ContainsKey(ticketIndex))
                            {
                                rulePositions.Add(ticketIndex, possibleRules.First());
                            }

                            break;
                        }
                    }
                }
            }

            return rulePositions;
        }
    }
}
