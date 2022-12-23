package com.example.fuel.utils.validation

import android.util.Patterns

class ValidatorEmail(private val email: String) : Validator(email) {

    enum class Error {
        ERROR_INCORRECT_EMAIL
    }

    var error: Error? = null

    override fun validate() {
        if (!Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            error = Error.ERROR_INCORRECT_EMAIL
        }
    }
}