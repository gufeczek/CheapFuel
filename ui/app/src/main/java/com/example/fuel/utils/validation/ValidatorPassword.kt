package com.example.fuel.utils.validation

class ValidatorPassword(private val password: String, private val repeatedPassword: String) : Validator(password) {

    enum class Error {
        PASSWORD_TOO_SHORT,
        PASSWORD_TOO_LONG,
        PASSWORD_NO_UPPERCASE,
        PASSWORD_NO_LOWERCASE,
        PASSWORD_NO_DIGIT,
        PASSWORD_REPEAT_NO_MATCH,
        PASSWORD_ILLEGAL_CHARACTER
    }
    var error: Error? = null

    override fun validate() {
        if (password.length < 8) {
            error = Error.PASSWORD_TOO_SHORT
        } else if (password.length > 32) {
            error = Error.PASSWORD_TOO_LONG
        } else if (!isAtLeastOneUpperCase(password)) {
            error = Error.PASSWORD_NO_UPPERCASE
        } else if (!isAtLeastOneLowerCase(password)) {
            error = Error.PASSWORD_NO_LOWERCASE
        } else if (!isAtLeastOneDigit(password)) {
            error = Error.PASSWORD_NO_DIGIT
        } else if (password != repeatedPassword) {
            error = Error.PASSWORD_REPEAT_NO_MATCH
        } else if (isIllegalCharacter(password)) {
            error = Error.PASSWORD_ILLEGAL_CHARACTER
        }
    }
}