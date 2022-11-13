package com.example.fuel.utils

import android.util.Log
import kotlinx.coroutines.flow.MutableSharedFlow
import java.util.concurrent.atomic.AtomicInteger

class DataLoadedIndicator(private val count: Int = 0) {
    private val publisher = MutableSharedFlow<Boolean>()

    private var loaded = AtomicInteger(0)

    suspend fun inc() {
        if (loaded.incrementAndGet() == count) {
            Log.d("XXDD", "tutaj")
            publisher.emit(true)
        }
    }

    fun isAllDataLoaded(): MutableSharedFlow<Boolean> {
        return publisher
    }
}