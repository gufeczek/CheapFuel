package com.example.fuel.utils.validation

class ValidatorNumber(val number: Double?, val min: Double?, val max: Double?) {

    fun validate(): Boolean =
        number != null &&
        (min == null || number >= min) &&
        (max == null || number <= max)
}