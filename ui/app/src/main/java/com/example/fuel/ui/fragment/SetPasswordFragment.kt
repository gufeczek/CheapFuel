package com.example.fuel.ui.fragment

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.navigation.Navigation
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetPasswordBinding
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.validation.ValidationPassword
import com.example.fuel.utils.validation.Validator.Companion.isAtLeastOneDigit
import com.example.fuel.utils.validation.Validator.Companion.isAtLeastOneUpperCase

class SetPasswordFragment : Fragment(R.layout.fragment_set_password) {

    private var _binding: FragmentSetPasswordBinding? = null
    private val binding get() = _binding!!
    private var error: ValidationPassword.Error? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentSetPasswordBinding.inflate(inflater, container, false)
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
        binding.clMain.setOnClickListener {view -> view.hideKeyboard()}

        binding.etPassword.afterTextChanged { editable ->
            passwordCheckmarks(editable)
        }
        binding.btnNextPage.setOnClickListener(btnNextPageListener)

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private val btnNextPageListener = View.OnClickListener {
        binding.tvPasswordValidationError.text = ""
        binding.tvRepeatedPasswordValidationError.text = ""
        binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded)
        binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded)

        val validator = ValidationPassword(
            binding.etPassword.text.toString(),
            binding.etRepeatPassword.text.toString()
        )
        validator.validate()
        error = validator.error

        if (error == null) {
            Navigation.findNavController(binding.root).navigate(R.id.blankFragment)
        } else {
            binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded_error)
            if (error == ValidationPassword.Error.PASSWORD_REPEAT_NO_MATCH) {
                binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded_error)
                binding.tvRepeatedPasswordValidationError.text = error.toString()
            } else {
                binding.tvPasswordValidationError.text = error.toString()
            }
        }
    }


    private fun passwordCheckmarks(text: String) {
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

}