package com.example.fuel.utils.converters

import java.math.RoundingMode
import java.text.DecimalFormat
import java.text.DecimalFormatSymbols
import java.time.LocalDateTime
import java.time.ZoneId
import java.util.*

object Converter {

    fun toCurrency(price: Double): String {
        val decSymbols = DecimalFormatSymbols(Locale.getDefault())
        val dec = DecimalFormat("#.00", decSymbols)
        dec.roundingMode = RoundingMode.CEILING
        return dec.format(price)
    }

    fun toLocalDateTime(date: Date): LocalDateTime {
        return date.toInstant()
            .atZone(ZoneId.systemDefault())
            .toLocalDateTime()
    }
}