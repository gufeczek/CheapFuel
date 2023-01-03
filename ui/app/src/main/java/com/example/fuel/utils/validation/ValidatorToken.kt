package com.example.fuel.utils.validation

import com.google.android.material.textfield.TextInputEditText

class ValidatorToken(private val token: Array<TextInputEditText>) : Validator(token) {

    enum class Error {
        ERROR_INCORRECT_TOKEN
    }

    var error: Error? = null

    override fun validate() {
        for (i in token.indices) {
            if (token[i].text.toString() == "") {
                error = Error.ERROR_INCORRECT_TOKEN
            }
        }
    }
}