package com.example.fuel.ui.utils

import android.content.res.Resources
import com.example.fuel.R
import com.example.fuel.model.Price
import com.example.fuel.utils.converters.Converter
import com.example.fuel.utils.extension.DurationExtension.Companion.toMonths
import com.example.fuel.utils.extension.DurationExtension.Companion.toYears
import java.time.Duration
import java.time.LocalDateTime
import java.util.*

object DateParser {

    fun parseReviewDate(date: Date, resources: Resources): String {
        val parsedDate = Converter.toLocalDateTime(date)
        val diff = Duration.between(parsedDate, LocalDateTime.now())

        if (diff.toYears() == 1L) return resources.getString(R.string.one_year_ago)
        if (diff.toYears() > 1) return resources.getString(R.string.years_ago, diff.toYears().toString())
        if (diff.toMonths() == 1L) return resources.getString(R.string.one_month_ago)
        if (diff.toMonths() > 1) return resources.getString(R.string.months_ago, diff.toMonths().toString())
        if (diff.toDays() == 1L) return resources.getString(R.string.one_day_ago)
        if (diff.toDays() > 1) return resources.getString(R.string.days_ago, diff.toDays().toString())
        if (diff.toHours() > 1) return resources.getString(R.string.hours_ago, diff.toHours().toString())

        return resources.getString(R.string.less_then_hour_ago)
    }

    fun parseFuelPriceCreatedAt(fuelPrice: Price?, resources: Resources): String {
        if (fuelPrice == null) return resources.getString(R.string.never)

        val createdAt = Converter.toLocalDateTime(fuelPrice.createdAt)
        val diff = Duration.between(createdAt, LocalDateTime.now())

        if (diff.toDays() == 1L) return resources.getString(R.string.one_day_ago)
        if (diff.toDays() > 1) return resources.getString(R.string.days_ago, diff.toDays().toString())
        if (diff.toHours() > 1) return resources.getString(R.string.hours_ago, diff.toHours().toString())

        return resources.getString(R.string.one_hour_ago)
    }
}