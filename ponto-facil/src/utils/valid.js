function isFieldValid(input) {
    return (input + '').trim() !== "";
}

function isNullOrEmpty(value) {
    if(value == null) return true
    return false
}

module.exports = {
    isFieldValid,
    isNullOrEmpty
}