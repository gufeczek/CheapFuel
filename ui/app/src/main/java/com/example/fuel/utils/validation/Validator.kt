package com.example.fuel.utils.validation

abstract class Validator(protected val text: Any ?= null) {
    abstract fun validate()

    companion object {
        fun isIllegalCharacter(text: String): Boolean {
            val regex = """^[a-zA-Z0-9!#${'$'}%&'()*+,-.:;<=>?@\[\]^_`{}|~]+${'$'}""".toRegex()
            return !regex.matches(text)
        }

        fun isAtLeastOneUpperCase(text: String): Boolean {
            for (i in text.indices) {
                if (text[i].isUpperCase()) {
                    return true
                }
            }
            return false
        }

        fun isAtLeastOneLowerCase(text: String): Boolean {
            for (i in text.indices) {
                if (text[i].isLowerCase()) {
                    return true
                }
            }
            return false
        }

        fun isAtLeastOneDigit(text: String): Boolean {
            for (i in text.indices) {
                if (text[i].isDigit()) {
                    return true
                }
            }
            return false
        }
    }
}