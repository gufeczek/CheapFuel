package com.example.fuel.ui.fragment

import android.app.Activity
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.navigation.Navigation
import androidx.navigation.findNavController
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetPasswordBinding
import com.example.fuel.databinding.FragmentSetUsernameBinding
import com.example.fuel.ui.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.ui.utils.extension.EditTextExtension.Companion.afterTextChanged
import java.util.*

class SetPasswordFragment : Fragment(R.layout.fragment_set_password) {

    private lateinit var binding: FragmentSetPasswordBinding

    private enum class Error {
        PASSWORD_TOO_SHORT,
        PASSWORD_TOO_LONG,
        PASSWORD_NO_UPPERCASE,
        PASSWORD_NO_LOWERCASE,
        PASSWORD_NO_DIGIT,
        PASSWORD_REPEAT_NO_MATCH,
        PASSWORD_ILLEGAL_CHARACTER
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentSetPasswordBinding.inflate(inflater, container, false)
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }

        binding.etPassword.afterTextChanged { editable ->
            validationPasswordCheckmarks(editable)
        }

        binding.btnNextPage.setOnClickListener {
            binding.tvPasswordValidationError.text = ""
            binding.tvRepeatedPasswordValidationError.text = ""
            binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded)
            binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded)

            val error = validationPassword(
                binding.etPassword.text.toString(),
                binding.etRepeatPassword.text.toString()
            ).orElse(null)

            if (error == null) {
                Navigation.findNavController(binding.root).navigate(R.id.blankFragment)
            } else {
                binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded_error)
                if (error == Error.PASSWORD_REPEAT_NO_MATCH) {
                    binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded_error)
                    binding.tvRepeatedPasswordValidationError.text = error.toString()
                } else {
                    binding.tvPasswordValidationError.text = error.toString()
                }
            }
        }

        binding.clMain.setOnClickListener {view -> view.hideKeyboard()}

        return binding.root
    }

    private fun validationPasswordCheckmarks(text: String) {
        if (text.length >= 8) {
            binding.imageAtLeast8Characters.setImageResource(R.drawable.ic_tick_green_32dp)
        } else {
            binding.imageAtLeast8Characters.setImageResource(R.drawable.ic_cancel_red_47dp)
        }

        if (isAtLeastOneUpperCase(text)) {
            binding.imageAtLeastOneCapitalLetter.setImageResource(R.drawable.ic_tick_green_32dp)
        } else {
            binding.imageAtLeastOneCapitalLetter.setImageResource(R.drawable.ic_cancel_red_47dp)
        }

        if (isAtLeastOneDigit(text)) {
            binding.imageAtLeastOneDigit.setImageResource(R.drawable.ic_tick_green_32dp)
        } else {
            binding.imageAtLeastOneDigit.setImageResource(R.drawable.ic_cancel_red_47dp)
        }
    }

    private fun validationPassword(password: String, repeatedPassword: String): Optional<Error> {
        if (password.length < 8) {
            return Optional.of(Error.PASSWORD_TOO_SHORT)
        } else if (password.length > 32) {
            return Optional.of(Error.PASSWORD_TOO_LONG)
        } else if (!isAtLeastOneUpperCase(password)) {
            return Optional.of(Error.PASSWORD_NO_UPPERCASE)
        } else if (!isAtLeastOneLowerCase(password)) {
            return Optional.of(Error.PASSWORD_NO_LOWERCASE)
        } else if (!isAtLeastOneDigit(password)) {
            return Optional.of(Error.PASSWORD_NO_DIGIT)
        } else if (password != repeatedPassword) {
            return Optional.of(Error.PASSWORD_REPEAT_NO_MATCH)
        } else if (isIllegalCharacter(password)) {
            return Optional.of(Error.PASSWORD_ILLEGAL_CHARACTER)
        }
        return Optional.empty()
    }

    private fun isIllegalCharacter(text: String): Boolean {
        val regex = """^[a-zA-Z0-9!#${'$'}%&'()*+,-.:;<=>?@\[\]^_`{}|~]+${'$'}""".toRegex()
        return !regex.matches(text)
    }

    private fun isAtLeastOneUpperCase(text: String): Boolean {
        for (i in text.indices) {
            if (text[i].isUpperCase()) {
                return true
            }
        }
        return false
    }

    private fun isAtLeastOneLowerCase(text: String): Boolean {
        for (i in text.indices) {
            if (text[i].isLowerCase()) {
                return true
            }
        }
        return false
    }

    private fun isAtLeastOneDigit(text: String): Boolean {
        for (i in text.indices) {
            if (text[i].isDigit()) {
                return true
            }
        }
        return false
    }
}