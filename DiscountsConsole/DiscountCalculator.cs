using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DiscountsConsole
{
    public class DiscountCalculator
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to the Retail Discount Calculator!");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();

            double billAmount = GetDoubleInput("Enter the total bill amount: ");
            bool hasGoldCard = GetBoolInput("Do you have a gold card? (y/n): ");
            bool hasSilverCard = GetBoolInput("Do you have a silver card? (y/n): ");
            bool isAffiliate = GetBoolInput("Are you an affiliate? (y/n): ");
            bool isLongTermCustomer = GetBoolInput("Have you been a customer for over 2 years? (y/n): ");

            var dto = new BillPaymentRequestDto
            {
                BillAmount = billAmount,
                HasGoldCard = hasGoldCard,
                HasSilverCard = hasSilverCard,
                IsAffiliate = isAffiliate,
                IsLongTermCustomer = isLongTermCustomer
            };

            var result = Calculator(dto);

            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.WriteLine("      Order Summary");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Bill Amount: ${0:F2}", result.BillAmount);
            Console.WriteLine("Total Discount: ${0:F2}", result.TotalDiscount);
            Console.WriteLine("Net Payable Amount: ${0:F2}", result.NetPayableAmount);
            Console.WriteLine("------------------------------");

            bool createUml = GetBoolInput("Do you want to generate the UML class diagram? (y/n):");

            if (createUml)
            {
                // Set the path to the output directory where the generated diagram will be saved
                string outputDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

                // Set the namespace of the classes to be diagrammed
                string targetNamespace = typeof(DiscountCalculator).Namespace;

                // Generate the UML class diagram for the specified namespace
                GenerateClassDiagram(targetNamespace, outputDirectory);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Generates a UML class diagram for the specified target namespace and saves it to the specified output directory.
        /// </summary>
        /// <param name="targetNamespace">The target namespace to generate the diagram for.</param>
        /// <param name="outputDirectory">The directory to save the generated diagram file.</param>
        private static void GenerateClassDiagram(string targetNamespace, string outputDirectory)
        {
            StringBuilder sb = new StringBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Add the PlantUML header and configuration
            sb.AppendLine("@startuml");
            sb.AppendLine("skinparam classAttributeIconSize 0");
            sb.AppendLine();

            // Get the types from the assembly that are in the target namespace
            Type[] types = assembly.GetTypes().Where(t => t.Namespace == targetNamespace).ToArray();

            // Process each type and generate PlantUML code
            foreach (Type type in types)
            {
                // Exclude compiler-generated types
                if (type.Name.StartsWith("<>c__DisplayClass"))
                {
                    continue;
                }

                // Add the class declaration
                sb.AppendLine($"class {type.Name} {{");

                // Add the class properties
                foreach (PropertyInfo property in type.GetProperties())
                {
                    sb.AppendLine($"  {property.Name} : {property.PropertyType.Name}");
                }

                // Add the class methods
                foreach (MethodInfo method in type.GetMethods())
                {
                    sb.AppendLine($"  {method.Name}()");
                }

                sb.AppendLine("}");
                sb.AppendLine();
            }

            // Add the PlantUML footer
            sb.AppendLine("@enduml");

            // Save the generated diagram code to a file
            string outputFilePath = Path.Combine(outputDirectory, "class_diagram.puml");
            File.WriteAllText(outputFilePath, sb.ToString());

            Console.WriteLine("UML class diagram generated successfully!");
            Console.WriteLine("Output file: " + outputFilePath);
        }

        /// <summary>
        /// Utility method to get a double input from the user.
        /// </summary>
        /// <param name="prompt">The prompt message to display to the user.</param>
        /// <returns>The double value entered by the user.</returns>
        private static double GetDoubleInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            double value;
            while (!double.TryParse(input, out value))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Console.Write(prompt);
                input = Console.ReadLine();
            }
            return value;
        }

        /// <summary>
        /// Utility method to get a boolean input from the user.
        /// </summary>
        /// <param name="prompt">The prompt message to display to the user.</param>
        /// <returns>The boolean value entered by the user.</returns>
        private static bool GetBoolInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine().ToLower();
            while (input != "y" && input != "n")
            {
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                Console.Write(prompt);
                input = Console.ReadLine().ToLower();
            }
            return input == "y";
        }

        /// <summary>
        /// Calculates the bill payment details based on the provided input.
        /// </summary>
        /// <param name="billPaymentRequestDto">The bill payment request DTO.</param>
        /// <returns>The bill payment response DTO with the calculated details.</returns>
        private static BillPaymentResponseDto Calculator(BillPaymentRequestDto billPaymentRequestDto)
        {
            double discount = 0.0;

            if (billPaymentRequestDto.HasGoldCard)
            {
                discount = 0.3; // 30% discount for gold cardholders 
            }
            else if (billPaymentRequestDto.HasSilverCard)
            {
                discount = 0.2; // 20% discount for silver cardholders
            }
            else if (billPaymentRequestDto.IsAffiliate)
            {
                discount = 0.1; // 10% discount for affiliates
            }
            else if (billPaymentRequestDto.IsLongTermCustomer)
            {
                discount = 0.05; // 5% discount for long-term customers
            }

            // Calculate $5 discount for every $200 on the bill
            double cashDiscount = Math.Floor(billPaymentRequestDto.BillAmount / 200) * 5;

            double totalDiscount = billPaymentRequestDto.BillAmount * discount + cashDiscount;

            double netPayableAmount = billPaymentRequestDto.BillAmount - totalDiscount;

            return new BillPaymentResponseDto
            {
                BillAmount = billPaymentRequestDto.BillAmount,
                TotalDiscount = totalDiscount,
                NetPayableAmount = netPayableAmount
            };
        }
    }
}
