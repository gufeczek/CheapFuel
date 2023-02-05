package com.example.fuel.viewmodel

import android.content.res.Resources
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.enums.AccountStatus
import com.example.fuel.enums.Role
import com.example.fuel.model.UserDetails
import com.example.fuel.model.account.ChangePassword
import com.example.fuel.model.block.BlockUser
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.model.review.Review
import com.example.fuel.repository.BlockUserRepository
import com.example.fuel.repository.ReviewRepository
import com.example.fuel.repository.UserRepository
import com.example.fuel.ui.utils.DateParser
import com.example.fuel.utils.extension.DurationExtension.Companion.areClose
import kotlinx.coroutines.launch
import retrofit2.Response
import java.text.SimpleDateFormat
import java.time.Duration
import java.util.*

class UserDetailsViewModel(
    private val userRepository: UserRepository,
    private val reviewRepository: ReviewRepository,
    private val blockUserRepository: BlockUserRepository): ViewModel() {

    private val reviewSortBy = "CreatedAt"
    private val reviewSortDirection = "Desc"

    var user: MutableLiveData<Response<UserDetails>> = MutableLiveData()
    var reviews: MutableLiveData<Response<Page<Review>>> = MutableLiveData()
    var deleteReview: MutableLiveData<Response<Void>> = MutableLiveData()
    var blockUser: MutableLiveData<Response<BlockUser>> = MutableLiveData()
    var deactivateUser: MutableLiveData<Response<Void>> = MutableLiveData()
    var changePasswordResponse: MutableLiveData<Response<Void>> = MutableLiveData()

    fun getUser(username: String) {
        viewModelScope.launch {
            user.value = userRepository.getUserDetails(username)
        }
    }

    fun getFirstPageOfReviews(username: String) {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, 10, reviewSortBy, reviewSortDirection)
            reviews.value = reviewRepository.getUserReviews(username, pageRequest)
        }
    }

    fun getNextPageOfUserReviews(username: String) {
        viewModelScope.launch {
            val nextPage = (if (reviews.value != null) reviews.value?.body()?.nextPage else 1) ?: return@launch

            val pageRequest = PageRequest(nextPage, 10, "CreatedAt", "Desc")
            reviews.value = reviewRepository.getUserReviews(username, pageRequest)
        }
    }

    fun deleteUserReview(reviewId: Long) {
        viewModelScope.launch {
            deleteReview.value = reviewRepository.deleteFuelStationReview(reviewId)
        }
    }

    fun blockUser(username: String, reason: String) {
        viewModelScope.launch {
            blockUser.value = blockUserRepository.blockUser(BlockUser(username, reason))
        }
    }

    fun deactivateUser(username: String) {
        viewModelScope.launch {
            deactivateUser.value = userRepository.deactivateUser(username)
        }
    }

    fun changePassword(changePassword: ChangePassword) {
        viewModelScope.launch {
            changePasswordResponse.value = userRepository.changePassword(changePassword)
        }
    }

    fun hasMoreReviews(): Boolean = reviews.value?.body()?.nextPage != null

    fun getUsername(): String {
        return user.value!!.body()!!.username
    }

    fun getAccountStatus(): AccountStatus {
        val status = user.value!!.body()!!.status
        return AccountStatus.fromString(status)
    }

    fun getCreatedAt(): String {
        val createdAt = user.value!!.body()!!.createdAt
        val formatter = SimpleDateFormat("dd.MM.yyyy")
        return formatter.format(createdAt)
    }

    fun getEmail(): String {
        return user.value!!.body()!!.email
    }

    fun isEmailConfirmed(): Boolean {
        return user.value!!.body()!!.emailConfirmed
    }

    fun getRole(): Role {
        val role = user.value!!.body()!!.role
        return Role.fromString(role)
    }

    fun parseReviewDate(date: Date, resources: Resources): String =
        DateParser.parseReviewDate(date, resources)

    fun hasReviewContent(review: Review): Boolean = review.content != null

    fun hasReviewBeenEdited(review: Review): Boolean =
        !Duration
            .between(review.createdAt.toInstant(), review.updatedAt.toInstant())
            .areClose(Duration.ofMillis(500))

    fun clear() {
        user = MutableLiveData()
        reviews = MutableLiveData()
        deleteReview = MutableLiveData()
        blockUser = MutableLiveData()
        deactivateUser = MutableLiveData()
        changePasswordResponse = MutableLiveData()
    }
}