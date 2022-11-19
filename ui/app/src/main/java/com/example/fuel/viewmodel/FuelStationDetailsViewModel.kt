package com.example.fuel.viewmodel

import android.content.res.Resources
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.R
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.Price
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.utils.converters.Converter
import kotlinx.coroutines.launch
import retrofit2.Response
import java.time.Duration
import java.time.LocalDateTime

class FuelStationDetailsViewModel(
    private val fuelStationRepository: FuelStationRepository): ViewModel() {

    var fuelStationDetails: MutableLiveData<Response<FuelStationDetails>> = MutableLiveData()

    fun getFuelStationDetails(fuelStationId: Long) {
        viewModelScope.launch {
            fuelStationDetails.value = fuelStationRepository.getFuelStationDetails(fuelStationId)
        }
    }

    fun clear() {
        fuelStationDetails = MutableLiveData()
    }

    fun parsePrice(fuelPrice: Price?, resources: Resources): String {
        if (fuelPrice == null) return "-"

        return resources.getString(R.string.zloty, Converter.toCurrency(fuelPrice.price))
    }

    fun parseCreatedAt(fuelPrice: Price?, resources: Resources): String {
        if (fuelPrice == null) return resources.getString(R.string.never)

        val createdAt = Converter.toLocalDateTime(fuelPrice.createdAt)
        val diff = Duration.between(createdAt, LocalDateTime.now())

        if (diff.toDays() == 1L) return resources.getString(R.string.one_day_ago)
        if (diff.toDays() > 1) return resources.getString(R.string.days_ago, diff.toDays().toString())
        if (diff.toHours() > 1) return resources.getString(R.string.hours_ago, diff.toHours().toString())

        return resources.getString(R.string.less_then_hour_ago)
    }
}