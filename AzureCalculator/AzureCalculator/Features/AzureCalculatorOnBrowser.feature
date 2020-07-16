Feature: AzureCalculatorOnBrowser
	Via Azure Calculator site https://calculator-web.azurewebsites.net/, I can do my math operation with 2 numbers

Background:
	Given I am on the Azure Calculator site

Scenario Outline: Adding numbers
	When '<leftNumber>' 'adding' '<rightNumber>' via calculator UI
	Then '<resultNumber>' is returned
	Examples:
	| desc         | leftNumber | rightNumber | resultNumber |
	| Add_1_digits | 9          | 8           | 17           |
	| Add_2_digits | 99         | 90          | 189          |
	| Add_3_digits | 999        | 111         | 1110         |
	| Add_4_digits | 9999       | 1111        | 11110        |
	| Add_negative | -50        | -99         | -149         |
	| Add_decimal  | 0.11       | 0.22        | 0.33         |

Scenario Outline: Subtracting numbers
	When '<leftNumber>' 'subtracting' '<rightNumber>' via calculator UI
	Then '<resultNumber>' is returned
	Examples:
	| desc         | leftNumber | rightNumber | resultNumber |
	| Sub_1_digits | 9          | 8           | 1            |
	| Sub_2_digits | 10         | 50          | -40          |
	| Sub_3_digits | 456        | 356         | 100          |
	| Sub_4_digits | 9999       | 1111        | 8888         |
	| Sub_negative | 9999       | -1          | 10000        |
	| Sub_decimal  | 3.5        | 3.6         | -0.1         |


Scenario Outline: Multiplying numbers
	When '<leftNumber>' 'multiplying' '<rightNumber>' via calculator UI
	Then '<resultNumber>' is returned
	Examples:
	| desc           | leftNumber | rightNumber | resultNumber |
	| Mul_0_1        | 0          | 1           | 0            |
	| Mul_0_Negative | 0          | -99         | 0            |
	| Mul_1_digits   | 9          | 8           | 72           |
	| Mul_2_digits   | 11         | 20          | 220          |
	| Mul_3_digits   | 100        | 789         | 78900        |
	| Mul_4_digits   | 1111       | 9999        | 11108889     |
	| Mul_negative_1 | 999        | -10         | -990         |
	| Mul_negative_2 | -20        | -10         | 200          |
	| Mul_decimal    | -3         | 1.2         | -3.6         |

Scenario Outline: Dividing numbers
	When '<leftNumber>' 'dividing' '<rightNumber>' via calculator UI
	Then '<resultNumber>' is returned
	Examples:
	| desc         | leftNumber | rightNumber | resultNumber |
	| Div_0_1      | 0          | 1           | 0            |
	| Div_1_0      | 1          | 0           | N/A          |
	| Div_1_digits | 9          | 3           | 3            |
	| Div_2_digits | 99         | 3           | 33           |
	| Div_3_digits | 468        | 2           | 234          |
	| Div_4_digits | 4444       | 1111        | 4            |
	| Div_negative | 9999       | -33         | -303         |
	| Div_decimal  | 999        | 0.5         | 1998         |