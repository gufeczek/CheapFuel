package com.example.fuel.viewmodel

import android.view.View
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.account.UserLogin
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.validation.ValidatorPassword
import com.example.fuel.utils.validation.ValidatorUsername
import kotlinx.coroutines.launch
import retrofit2.Response

class UserLoginViewModel(private val repository: UserRepository) : ViewModel() {
    val response: MutableLiveData<Response<UserLogin>> = MutableLiveData()
    val user: MutableLiveData<UserLogin> = MutableLiveData()

    fun postLogin(user: UserLogin) {
        viewModelScope.launch {
            response.value = repository.postLogin(user)
        }
    }

    fun getUsernameValidationError(username: String): ValidatorUsername.Error? {
        val validator = ValidatorUsername(username)
        validator.validate()
        return validator.error
    }

    fun getPasswordValidationError(password: String): ValidatorPassword.Error? {
        val validator = ValidatorPassword(password, password)
        validator.validate()
        return validator.error
    }

    fun navigateToTBAFragment(view: View) {

    }
}