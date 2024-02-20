const number = 9.045;
const decimalPlaces = 2;

const roundedNumber = +(Math.round(`${number}e${decimalPlaces}`) + `e-${decimalPlaces}`);

const formattedNumber = roundedNumber.toFixed(decimalPlaces); // Add 1 to include the digit before the decimal point

console.log("roundedNumber", roundedNumber);
console.log("formattedNumber", formattedNumber);
