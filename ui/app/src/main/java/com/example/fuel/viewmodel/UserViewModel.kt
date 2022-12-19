package com.example.fuel.viewmodel

import android.view.View
import android.widget.Toast
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.model.User
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.validation.ValidationPassword
import com.example.fuel.utils.validation.ValidatorEmail
import com.example.fuel.utils.validation.ValidatorUsername
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

    fun navigateToPasswordFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.PasswordFragment)
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

    fun getPasswordValidationEror(password: String, repeatedPassword: String): ValidationPassword.Error? {
        val validator = ValidationPassword(password, repeatedPassword)
        validator.validate()
        return validator.error
    }
}