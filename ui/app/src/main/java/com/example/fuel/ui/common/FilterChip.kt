package com.example.fuel.ui.common

import android.content.Context
import androidx.core.content.ContextCompat
import com.example.fuel.R
import com.google.android.material.chip.Chip

private const val STROKE_WIDTH = 2F

fun initChipAppearance(chip: Chip, context: Context) {
    chip.chipStrokeWidth = STROKE_WIDTH
    chip.setTextColor(ContextCompat.getColor(context, R.color.gray))
}

fun toggleChipAppearance(chip: Chip, context: Context) {
    if (chip.isChecked) {
        chip.chipStrokeWidth = 0F
        chip.setTextColor(ContextCompat.getColor(context, R.color.dark_blue))
    } else {
        chip.chipStrokeWidth = STROKE_WIDTH
        chip.setTextColor(ContextCompat.getColor(context, R.color.gray))
    }
}