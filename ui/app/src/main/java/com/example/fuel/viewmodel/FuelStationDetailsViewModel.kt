package com.example.fuel.viewmodel

import android.content.res.Resources
import android.location.Location
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.R
import com.example.fuel.enums.Role
import com.example.fuel.utils.Auth
import com.example.fuel.model.review.Review
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelTypeWithPrice
import com.example.fuel.model.Price
import com.example.fuel.model.favourite.UserFavourite
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.model.price.NewFuelPrice
import com.example.fuel.model.price.NewFuelPriceResponse
import com.example.fuel.model.price.NewPriceAtFuelStationWithLocation
import com.example.fuel.model.price.NewPricesAtFuelStation
import com.example.fuel.model.report.Report
import com.example.fuel.model.review.NewReview
import com.example.fuel.model.review.UpdateReview
import com.example.fuel.repository.FavouriteRepository
import com.example.fuel.repository.FuelPriceRepository
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.ReportRepository
import com.example.fuel.repository.ReviewRepository
import com.example.fuel.ui.utils.DateParser
import com.example.fuel.utils.calculateDistance
import com.example.fuel.utils.converters.Converter
import com.example.fuel.utils.extension.DurationExtension.Companion.areClose
import com.example.fuel.viewmodel.mediator.FavouriteViewModelMediator
import com.example.fuel.viewmodel.mediator.ListViewModelMediator
import com.example.fuel.viewmodel.mediator.MapViewModelMediator
import kotlinx.coroutines.launch
import retrofit2.Response
import java.time.Duration
import java.util.Date

class FuelStationDetailsViewModel(
    private val fuelStationRepository: FuelStationRepository,
    private val reviewRepository: ReviewRepository,
    private val favouriteRepository: FavouriteRepository,
    private val fuelPriceRepository: FuelPriceRepository,
    private val reportRepository: ReportRepository): ViewModel() {

    private val allowedDistance = 200

    var fuelStationDetails: MutableLiveData<Response<FuelStationDetails>> = MutableLiveData()
    var fuelStationReviews: MutableLiveData<Response<Page<Review>>> = MutableLiveData()
    var deleteFuelStation: MutableLiveData<Response<Void>> = MutableLiveData()
    var userReview: MutableLiveData<Response<Review>> = MutableLiveData()
    var newUserReview: MutableLiveData<Response<Review>> = MutableLiveData()
    var updateUserReview: MutableLiveData<Response<Review>> = MutableLiveData()
    var deleteUserReview: MutableLiveData<Response<Void>> = MutableLiveData()
    var deleteDiffUserReview: MutableLiveData<Response<Void>> = MutableLiveData()
    var userFavourite: MutableLiveData<Response<UserFavourite>> = MutableLiveData()
    var addToFavourite: MutableLiveData<Response<UserFavourite>> = MutableLiveData()
    var deleteFavourite: MutableLiveData<Response<Void>> = MutableLiveData()
    var createNewFuelPrices: MutableLiveData<Response<Array<NewFuelPriceResponse>>> = MutableLiveData()
    var reportReview: MutableLiveData<Response<Report>> = MutableLiveData()


    fun getFuelStationDetails(fuelStationId: Long) {
        viewModelScope.launch {
            fuelStationDetails.value = fuelStationRepository.getFuelStationDetails(fuelStationId)
        }
    }

    fun deleteFuelStation(fuelStationId: Long) {
        viewModelScope.launch {
            deleteFuelStation.value = fuelStationRepository.deleteFuelStation(fuelStationId)

            MapViewModelMediator.fuelStationChanged()
            ListViewModelMediator.fuelStationChanged()
            FavouriteViewModelMediator.favouriteChanged()
        }
    }

    fun getFirstPageOfFuelStationReviews(fuelStationId: Long) {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, 10, "CreatedAt", "Desc")
            fuelStationReviews.value = reviewRepository.getFuelStationReviews(fuelStationId, pageRequest)
        }
    }

    fun getNextPageOfFuelStationReviews(fuelStationId: Long) {
        viewModelScope.launch {
            val nextPage = (if (fuelStationReviews.value != null) fuelStationReviews.value?.body()?.nextPage else 1) ?: return@launch

            val pageRequest = PageRequest(nextPage, 10, "CreatedAt", "Desc")
            fuelStationReviews.value = reviewRepository.getFuelStationReviews(fuelStationId, pageRequest)
        }
    }

    fun getUserFavourite(fuelStationId: Long) {
        viewModelScope.launch {
            userFavourite.value = favouriteRepository.getUserFavourite(Auth.username, fuelStationId)
        }
    }

    fun getFuelStationId(): Long? = fuelStationDetails.value?.body()?.id

    fun getFuelStation(): FuelStationDetails? = fuelStationDetails.value?.body();

    fun getFuelTypes(): Array<FuelTypeWithPrice>? = fuelStationDetails.value?.body()?.fuelTypes

    fun hasAnyServices(): Boolean = !fuelStationDetails.value?.body()?.services.isNullOrEmpty()

    fun hasMoreReviews(): Boolean = fuelStationReviews.value?.body()?.nextPage != null

    fun isAdmin(): Boolean = Auth.role == Role.ADMIN

    fun isFuelStationOwner(): Boolean = Auth.role == Role.OWNER
            && fuelStationDetails.value?.body()?.owners?.contains(Auth.username) == true

    fun isCloseToFuelStation(userLocation: Location?): Boolean {
        if (userLocation == null || getFuelStation() == null) return false

        val fuelStationLocation = getFuelStation()!!.location

        return calculateDistance(
            userLocation.latitude,
            userLocation.longitude,
            fuelStationLocation.latitude,
            fuelStationLocation.longitude) <= allowedDistance
    }

    fun parsePrice(fuelPrice: Price?, resources: Resources): String {
        if (fuelPrice == null) return "-"

        return resources.getString(R.string.zloty, Converter.toCurrency(fuelPrice.price))
    }

    fun parseFuelPriceCreatedAt(fuelPrice: Price?, resources: Resources): String =
        DateParser.parseFuelPriceCreatedAt(fuelPrice, resources)

    fun parseReviewDate(date: Date, resources: Resources): String =
        DateParser.parseReviewDate(date, resources)

    fun hasReviewContent(review: Review): Boolean = review.content != null

    fun hasReviewBeenEdited(review: Review): Boolean =
        !Duration
        .between(review.createdAt.toInstant(), review.updatedAt.toInstant())
        .areClose(Duration.ofMillis(500))

    fun isReviewValid(rate: Int, content: String?): Boolean =
        rate in 1..5 && (content == null || content.length <= 300)

    fun getUserReview(fuelStationId: Long) {
        viewModelScope.launch {
            userReview.value = reviewRepository.getUserReviewOfFuelStation(fuelStationId)
        }
    }

    fun createNewReviewForUser(rate: Int, content: String?) {
        if (fuelStationDetails.value?.body()?.id == null) return

        val newReview = NewReview(rate, content, fuelStationDetails.value!!.body()!!.id)
        viewModelScope.launch {
            newUserReview.value = reviewRepository.createFuelStationReview(newReview)
        }
    }

    fun editUserReview(rate: Int, content: String?) {
        if (userReview.value?.body() == null) return

        val reviewId = userReview.value!!.body()!!.id
        val review = UpdateReview(rate, content)

        viewModelScope.launch {
            updateUserReview.value = reviewRepository.editFuelStationReview(reviewId, review)
        }
    }

    fun deleteUserReview() {
        if (userReview.value?.body() == null) return

        val reviewId = userReview.value!!.body()!!.id
        viewModelScope.launch {
            deleteUserReview.value = reviewRepository.deleteFuelStationReview(reviewId)
        }
    }

    fun deleteUserReview(reviewId: Long) {
        viewModelScope.launch {
            deleteDiffUserReview.value = reviewRepository.deleteFuelStationReview(reviewId)
        }
    }

    fun addFuelStationToFavourite(fuelStationId: Long) {
        viewModelScope.launch {
            addToFavourite.value = favouriteRepository.addToFavourite(fuelStationId)
            FavouriteViewModelMediator.favouriteChanged()
        }
    }

    fun removeFuelStationFromFavourite(fuelStationId: Long) {
        viewModelScope.launch {
            deleteFavourite.value = favouriteRepository.deleteFromFavourite(fuelStationId)
            FavouriteViewModelMediator.favouriteChanged()
        }
    }

    fun createNewFuelPrices(fuelPrices: Array<NewFuelPrice>, userLocation: Location?) {
        if (getFuelStationId() == null) return

        if (Auth.role == Role.OWNER || Auth.role == Role.ADMIN) {
            createNewFuelPricesByOwner(fuelPrices)
        } else {
            createNewFuelPricesByUser(fuelPrices, userLocation)
        }
    }

    private fun createNewFuelPricesByOwner(fuelPrices: Array<NewFuelPrice>) {
        val fuelPricesAtStation = NewPricesAtFuelStation(getFuelStationId()!!, fuelPrices)

        viewModelScope.launch {
            createNewFuelPrices.value =
                fuelPriceRepository.createNewFuelPricesByOwner(fuelPricesAtStation)

            MapViewModelMediator.fuelStationChanged()
            ListViewModelMediator.fuelStationChanged()
        }
    }

    private fun createNewFuelPricesByUser(fuelPrices: Array<NewFuelPrice>, userLocation: Location?) {
        if (userLocation == null) return

        val fuelPricesAtStation = NewPriceAtFuelStationWithLocation(
            getFuelStationId()!!,
            userLocation.longitude,
            userLocation.latitude,
            fuelPrices)

        viewModelScope.launch {
            createNewFuelPrices.value =
                fuelPriceRepository.createNewFuelPricesByUser(fuelPricesAtStation)

            MapViewModelMediator.fuelStationChanged()
            ListViewModelMediator.fuelStationChanged()
        }
    }

    fun reportReview(reviewId: Long) {
        viewModelScope.launch {
            reportReview.value = reportRepository.reportReview(reviewId)
        }
    }

    fun notifyAboutChanges() {
        MapViewModelMediator.act()
        ListViewModelMediator.act()
        FavouriteViewModelMediator.act()
    }

    fun clear() {
        fuelStationDetails = MutableLiveData()
        deleteFuelStation = MutableLiveData()
        fuelStationReviews = MutableLiveData()
        userReview = MutableLiveData()
        newUserReview = MutableLiveData()
        updateUserReview = MutableLiveData()
        deleteUserReview = MutableLiveData()
        deleteDiffUserReview = MutableLiveData()
        addToFavourite = MutableLiveData()
        deleteFavourite = MutableLiveData()
        createNewFuelPrices = MutableLiveData()
        reportReview = MutableLiveData()
    }
}