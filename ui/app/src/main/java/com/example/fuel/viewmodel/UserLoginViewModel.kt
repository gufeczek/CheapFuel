package com.example.fuel.viewmodel

import android.view.View
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.model.account.UserLogin
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.validation.ValidatorEmail
import com.example.fuel.utils.validation.ValidatorPassword
import com.example.fuel.utils.validation.ValidatorUsername
import kotlinx.coroutines.launch
import retrofit2.Response

class UserLoginViewModel(private val repository: UserRepository) : ViewModel() {
    val response: MutableLiveData<Response<UserLogin>> = MutableLiveData()
    val resetToken: MutableLiveData<Response<String>> = MutableLiveData()

    fun postLogin(user: UserLogin) {
        viewModelScope.launch {
            response.value = repository.postLogin(user)
        }
    }

    fun getPasswordResetToken(email: String) {
        viewModelScope.launch {
            resetToken.value = repository.postPasswordResetToken(email)
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

    fun getEmailValidationError(email: String): ValidatorEmail.Error? {
        val validator = ValidatorEmail(email)
        validator.validate()
        return validator.error
    }

    fun navigateToResetPasswordFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.resetPassword)
    }

    fun navigateToResetPasswordCodeFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.resetPasswordCodeFragment)
    }

    fun navigateToTBAFragment(view: View) {

    }
}