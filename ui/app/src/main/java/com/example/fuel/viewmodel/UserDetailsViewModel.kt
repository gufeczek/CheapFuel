package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.enums.AccountStatus
import com.example.fuel.enums.Role
import com.example.fuel.model.UserDetails
import com.example.fuel.model.page.Page
import com.example.fuel.model.review.Review
import com.example.fuel.repository.ReviewRepository
import com.example.fuel.repository.UserRepository
import kotlinx.coroutines.launch
import retrofit2.Response
import java.text.SimpleDateFormat

class UserDetailsViewModel(
    private val userRepository: UserRepository,
    private val reviewRepository: ReviewRepository): ViewModel() {

    var user: MutableLiveData<Response<UserDetails>> = MutableLiveData()
    var review: MutableLiveData<Response<Page<Review>>> = MutableLiveData()


    fun getUser(username: String) {
        viewModelScope.launch {
            user.value = userRepository.getUserDetails(username)
        }
    }

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
}