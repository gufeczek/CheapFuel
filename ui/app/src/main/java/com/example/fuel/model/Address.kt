package com.example.fuel.model

data class Address(
    val city: String,
    val street: String?,
    val streetNumber: String,
    val postalCode: String
) {

    override fun toString(): String {
        return "$city, ul. $street $streetNumber"
    }
}