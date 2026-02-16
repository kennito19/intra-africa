import { Buffer } from 'buffer'

const MAX_ENCODED_ID_LENGTH = 15

// Encode the integer ID to truncated Base64
export function encodeId(value) {
  const valueString = value.toString()
  const encodedValue = Buffer.from(valueString).toString('base64')

  // Truncate to the desired length
  const truncatedEncodedValue = encodedValue.slice(0, MAX_ENCODED_ID_LENGTH)
  return truncatedEncodedValue
}

// Decode the truncated Base64-encoded ID to integer
export function decodeId(encodedValue) {
  const valueString = Buffer.from(encodedValue, 'base64').toString('utf-8')

  // Check if the original value was an integer or a string
  const decodedValue = !isNaN(valueString)
    ? parseInt(valueString, 10)
    : valueString
  return decodedValue
}

// function splitValues(encodedString) {
//     // Split the string using the comma as the delimiter
//     const valuesArray = encodedString.split(',');

//     // Trim any extra whitespace from each value
//     const trimmedValues = valuesArray.map(value => value.trim());

//     return trimmedValues;
//   }

// export function decodeArrayToString(encodedArray) {
//   // Use the map function to decode each element in the array
//   const decodedArray = encodedArray?.map((encodedValue) =>
//     decodeId(encodedValue)
//   )

//   // Join the decoded array into a comma-separated string
//   const decodedString = decodedArray.join(',')

//   return decodedString
// }

function arrayToQueryString(array) {
  if (!Array.isArray(array)) {
    throw new Error('Input must be an array')
  }

  return array.join(',')
}

export function splitAndDecode(encodedString) {
  // Split the string using the comma as the delimiter
  const valuesArray = encodedString?.split(',')

  // Trim any extra whitespace from each value and decode
  const decodedValues = valuesArray?.map((encodedValue) =>
    decodeId(encodedValue?.trim())
  )
  if (decodedValues?.length > 0) {
    return arrayToQueryString(decodedValues)
  }
  return decodedValues
}

export function encodeArray(array) {
  if (!Array.isArray(array)) {
    throw new Error('Input must be an array')
  }

  const stringArray = array.map((element) => encodeId(element))
  return stringArray
}

// export function decodeArray(encodedArray) {
//   // Use the map function to decode each element in the array
//   const decodedArray = encodedArray.map((encodedValue) =>
//     decodeId(encodedValue)
//   )

//   return decodedArray
// }
