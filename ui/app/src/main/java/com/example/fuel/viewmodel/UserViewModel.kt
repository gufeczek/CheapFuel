package com.example.fuel.viewmodel

import android.view.View
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.model.User
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.validation.ValidatorEmail
import kotlinx.coroutines.launch
import retrofit2.Response

class UserViewModel(private val repository: UserRepository): ViewModel() {

    val response: MutableLiveData<Response<User>> = MutableLiveData()

    fun postLogin(user: User) {
        viewModelScope.launch {
            response.value = repository.postLogin(user)
        }
    }

    fun postRegister(user: User) {
        viewModelScope.launch {
            response.value = repository.postRegister(user)
        }
    }

    fun navigateToUsernameFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.UsernameFragment)
    }

    fun getEmailValidationError(email: String): ValidatorEmail.Error? {
        val validator = ValidatorEmail(email)
        validator.validate()
        return validator.error
    }
}