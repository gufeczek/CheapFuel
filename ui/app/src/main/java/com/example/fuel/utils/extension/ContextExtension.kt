package com.example.fuel.utils.extension

import android.content.Context
import android.view.View
import android.view.inputmethod.InputMethodManager

class ContextExtension {
    companion object {
        fun View.hideKeyboard() {
            val imm = context.getSystemService(Context.INPUT_METHOD_SERVICE) as InputMethodManager
            imm.hideSoftInputFromWindow(windowToken, 0)
        }
    }
}