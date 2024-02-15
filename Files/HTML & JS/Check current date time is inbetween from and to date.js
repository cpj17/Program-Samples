function isCurrentDateInRange(startDateTimeStr, endDateTimeStr) {
  const currentDate = new Date();
  const startDate = new Date(startDateTimeStr);
  const endDate = new Date(endDateTimeStr);

  return currentDate >= startDate && currentDate <= endDate;
}

// Example usage:
const startDateStr = '2023-12-06T14:12:00';
const endDateStr = '2023-12-06T20:32:00';

const result = isCurrentDateInRange(startDateStr, endDateStr);
console.log(result); // This will print either true or false
