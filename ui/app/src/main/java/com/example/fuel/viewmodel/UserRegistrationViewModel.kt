package com.example.fuel.viewmodel

import android.view.View
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.NavDirections
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.model.account.UserRegistration
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.validation.ValidatorPassword
import com.example.fuel.utils.validation.ValidatorEmail
import com.example.fuel.utils.validation.ValidatorUsername
import kotlinx.coroutines.launch
import retrofit2.Response

class UserRegistrationViewModel(private val repository: UserRepository): ViewModel() {

    var response: MutableLiveData<Response<UserRegistration>> = MutableLiveData()
    var user: MutableLiveData<UserRegistration> = MutableLiveData()

    fun postRegister(user: UserRegistration) {
        viewModelScope.launch {
            response.value = repository.postRegister(user)
        }
    }

    fun navigateToLoginFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.LoginFragment)
    }

    fun navigateToUsernameFragment(view: View, action: NavDirections) {
        Navigation.findNavController(view).navigate(action)
    }

    fun navigateToPasswordFragment(view: View, action: NavDirections) {
        Navigation.findNavController(view).navigate(action)
    }

    fun navigateToTBAFragment(view: View) {

    }

    fun getEmailValidationError(email: String): ValidatorEmail.Error? {
        val validator = ValidatorEmail(email)
        validator.validate()
        return validator.error
    }

    fun getUsernameValidationError(username: String): ValidatorUsername.Error? {
        val validator = ValidatorUsername(username)
        validator.validate()
        return validator.error
    }

    fun getPasswordValidationError(password: String, repeatedPassword: String): ValidatorPassword.Error? {
        val validator = ValidatorPassword(password, repeatedPassword)
        validator.validate()
        return validator.error
    }
}