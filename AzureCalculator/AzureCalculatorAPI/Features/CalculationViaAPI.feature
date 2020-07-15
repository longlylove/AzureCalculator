Feature: CalculationViaAPI
	Returning calculation result via Azure Calculator API https://calculator-api.azurewebsites.net/api/Calculate

Scenario Outline: Adding
	When '<leftNumber>' 'adding' '<rightNumber>' via API
	Then '<resultNumber>' is returned with http code '<httpCode>'
	Examples:
	| desc   | leftNumber | rightNumber | resultNumber | httpCode |
	| Add_01 | 1          | 1           | 2            | 200      |
	| Add_02 | 1          | 0           | 1            | 200      |
	| Add_03 | 0          | 0           | 0            | 200      |
	| Add_04 | 0          | 1           | 1            | 200      |
	| Add_05 | 50         | 99          | 149          | 200      |
	| Add_06 | 100        | 99          | 199          | 200      |
	| Add_07 | 999        | 9999        | 10998        | 200      |
	| Add_08 | -1         | 1           | 0            | 200      |
	| Add_09 | -100       | -99         | -199         | 200      |
	| Add_10 | -999999999 | 111111111   | -888888888   | 200      |
	| Add_11 | 20.79      | 51.11       | 71.90        | 200      |
	| Add_12 | -55.5      | 22.2        | -33.3        | 200      |

Scenario Outline: Subtracting
	When '<leftNumber>' 'subtracting' '<rightNumber>' via API
	Then '<resultNumber>' is returned with http code '<httpCode>'
	Examples:
	| desc   | leftNumber | rightNumber | resultNumber | httpCode |
	| Sub_01 | 1          | 1           | 0            | 200      |
	| Sub_02 | 1          | 0           | 1            | 200      |
	| Sub_03 | 0          | 0           | 0            | 200      |
	| Sub_04 | 0          | 1           | -1           | 200      |
	| Sub_05 | 50         | 99          | -49          | 200      |
	| Sub_06 | 100        | 99          | 1            | 200      |
	| Sub_07 | 999        | 9999        | -9000        | 200      |
	| Sub_08 | -1         | 1           | -2           | 200      |
	| Sub_09 | -100       | -99         | -1           | 200      |
	| Sub_10 | -999999999 | 111111111   | -1111111110  | 200      |
	| Sub_11 | 20.79      | 51.11       | -30.32       | 200      |
	| Sub_12 | -55.5      | 22.2        | -77.7        | 200      |

Scenario Outline: Multiplying
	When '<leftNumber>' 'multiplying' '<rightNumber>' via API
	Then '<resultNumber>' is returned with http code '<httpCode>'
	Examples:
	| desc   | leftNumber | rightNumber | resultNumber        | httpCode |
	| Mul_01 | 1          | 1           | 1                   | 200      |
	| Mul_02 | 1          | 0           | 0                   | 200      |
	| Mul_03 | 0          | 0           | 0                   | 200      |
	| Mul_04 | 0          | 1           | 0                   | 200      |
	| Mul_05 | 50         | 99          | 4950                | 200      |
	| Mul_06 | 100        | 99          | 9900                | 200      |
	| Mul_07 | 999        | 9999        | 9989001             | 200      |
	| Mul_08 | -1         | 1           | -1                  | 200      |
	| Mul_09 | -100       | -99         | 9900                | 200      |
	| Mul_10 | -999999999 | 111111111   | -111111110888888896 | 200      |
	| Mul_11 | 20.79      | 51.11       | 1062.5769           | 200      |
	| Mul_12 | -55.5      | 22.2        | -1231.1             | 200      |

Scenario Outline: Dividing
	When '<leftNumber>' 'dividing' '<rightNumber>' via API
	Then '<resultNumber>' is returned with http code '<httpCode>'
	Examples:
	| desc   | leftNumber | rightNumber | resultNumber       | httpCode |
	| Div_01 | 1          | 1           | 1                  | 200      |
	| Div_02 | 1          | 0           | N/A                | 500      |
	| Div_03 | 0          | 0           | N/A                | 500      |
	| Div_04 | 0          | 1           | 0                  | 200      |
	| Div_05 | 50         | 99          | 0.5050505050505051 | 200      |
	| Div_06 | 100        | 99          | 1.01010101010101   | 200      |
	| Div_07 | 999        | 9999        | 0.0999099909990999 | 200      |
	| Div_08 | -1         | 1           | -1                 | 200      |
	| Div_09 | -100       | -99         | 1.01010101010101   | 200      |
	| Div_10 | -999999999 | 111111111   | -9                 | 200      |
	| Div_11 | 20.79      | 51.11       | 0.4067697123850518 | 200      |
	| Div_12 | -55.5      | 22.2        | -2.5               | 200      |
