package com.example.fuel.utils.validation

class ValidatorUsername(private val username: String) : Validator(username) {

    enum class Error {
        ERROR_USERNAME_TOO_SHORT,
        ERROR_USERNAME_TOO_LONG
    }

    var error: Error? = null

    override fun validate(): Boolean {
        if (username.length < 3) {
            error = Error.ERROR_USERNAME_TOO_SHORT
            return false
        } else if (username.length > 32) {
            error = Error.ERROR_USERNAME_TOO_LONG
            return false
        }
        return true
    }
}