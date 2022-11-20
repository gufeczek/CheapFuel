package com.example.fuel.utils.extension

import java.time.Duration
import java.time.LocalDateTime
import java.time.temporal.ChronoUnit

class DurationExtension {
    companion object {
        fun Duration.toYears(): Long {
            return ChronoUnit.YEARS.between(LocalDateTime.now().minus(this), LocalDateTime.now())
        }

        fun Duration.toMonths(): Long {
            return ChronoUnit.MONTHS.between(LocalDateTime.now().minus(this), LocalDateTime.now())
        }
    }
}