package com.example.fuel.viewmodel

import android.content.res.Resources
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.R
import com.example.fuel.model.Review
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.Price
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.ReviewRepository
import com.example.fuel.utils.converters.Converter
import com.example.fuel.utils.extension.DurationExtension.Companion.toMonths
import com.example.fuel.utils.extension.DurationExtension.Companion.toYears
import kotlinx.coroutines.launch
import retrofit2.Response
import java.time.Duration
import java.time.LocalDateTime
import java.util.Date

class FuelStationDetailsViewModel(
    private val fuelStationRepository: FuelStationRepository,
    private val reviewRepository: ReviewRepository): ViewModel() {

    var fuelStationDetails: MutableLiveData<Response<FuelStationDetails>> = MutableLiveData()
    var fuelStationReviews: MutableLiveData<Response<Page<Review>>> = MutableLiveData()

    fun getFuelStationDetails(fuelStationId: Long) {
        viewModelScope.launch {
            fuelStationDetails.value = fuelStationRepository.getFuelStationDetails(fuelStationId)
        }
    }

    fun getNextPageOfFuelStationReviews(fuelStationId: Long) {
        viewModelScope.launch {
            val nextPage = (if (fuelStationReviews.value != null) fuelStationReviews.value?.body()?.nextPage else 1) ?: return@launch

            val pageRequest = PageRequest(nextPage, 10, "CreatedAt", "Desc")
            fuelStationReviews.value = reviewRepository.getFuelStationReviews(fuelStationId, pageRequest)
        }
    }

    fun hasAnyServices(): Boolean = !fuelStationDetails.value?.body()?.services.isNullOrEmpty()

    fun hasMoreReviews(): Boolean = fuelStationReviews.value?.body()?.nextPage != null

    fun parsePrice(fuelPrice: Price?, resources: Resources): String {
        if (fuelPrice == null) return "-"

        return resources.getString(R.string.zloty, Converter.toCurrency(fuelPrice.price))
    }

    fun parseFuelPriceCreatedAt(fuelPrice: Price?, resources: Resources): String {
        if (fuelPrice == null) return resources.getString(R.string.never)

        val createdAt = Converter.toLocalDateTime(fuelPrice.createdAt)
        val diff = Duration.between(createdAt, LocalDateTime.now())

        if (diff.toDays() == 1L) return resources.getString(R.string.one_day_ago)
        if (diff.toDays() > 1) return resources.getString(R.string.days_ago, diff.toDays().toString())
        if (diff.toHours() > 1) return resources.getString(R.string.hours_ago, diff.toHours().toString())

        return resources.getString(R.string.less_then_hour_ago)
    }

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

    fun hasReviewContent(review: Review): Boolean = review.content != null

    fun hasReviewBeenEdited(review: Review): Boolean = review.createdAt != review.updatedAt

    fun clear() {
        fuelStationDetails = MutableLiveData()
        fuelStationReviews = MutableLiveData()
    }
}