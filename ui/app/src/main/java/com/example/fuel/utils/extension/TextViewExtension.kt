package com.example.fuel.utils.extension

import android.text.SpannableString
import android.text.TextPaint
import android.text.style.URLSpan
import android.widget.TextView

open class TextViewExtension {
    companion object {
        fun TextView.removeLinksUnderline() {
            val spannable = SpannableString(text)
            for (urlSpan in spannable.getSpans(0, spannable.length, URLSpan::class.java)) {
                spannable.setSpan(object : URLSpan(urlSpan.url) {
                    override fun updateDrawState(ds: TextPaint) {
                        super.updateDrawState(ds)
                        ds.isUnderlineText = false
                    }
                }, spannable.getSpanStart(urlSpan), spannable.getSpanEnd(urlSpan), 0)
            }
            text = spannable
        }
    }
}