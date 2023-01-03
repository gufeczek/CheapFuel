package com.example.fuel.ui.fragment.login

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import androidx.navigation.fragment.navArgs
import com.example.fuel.R
import com.example.fuel.databinding.FragmentResetPasswordCodeBinding
import com.example.fuel.model.account.UserPasswordReset
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.validation.ValidatorPassword
import com.example.fuel.utils.validation.ValidatorToken
import com.example.fuel.viewmodel.UserLoginViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.textfield.TextInputEditText

class ResetPasswordCodeFragment : Fragment(R.layout.fragment_reset_password_code) {

    private var _binding: FragmentResetPasswordCodeBinding? = null
    private val binding get() = _binding!!
    private lateinit var viewModel: UserLoginViewModel
    private lateinit var codes: Array<TextInputEditText>
    private var errorPassword: ValidatorPassword.Error? = null
    private var errorToken: ValidatorToken.Error? = null
    private val args: ResetPasswordCodeFragmentArgs by navArgs()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentResetPasswordCodeBinding.inflate(inflater, container, false)
        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserLoginViewModel::class.java]

        binding.clMain.setOnClickListener { view -> view.hideKeyboard() }
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
        binding.btnNextPage.setOnClickListener(btnGoToNextPage)

        codes = arrayOf(
            binding.etFirst,
            binding.etSecond,
            binding.etThird,
            binding.etFourth,
            binding.etFifth,
            binding.etSixth
        )
        viewModel.setupCodeInputLogic(codes)

        return binding.root
    }

    override fun onDestroy() {
        super.onDestroy()
        _binding = null
    }

    private val btnGoToNextPage = View.OnClickListener { view ->
        hidePasswordError()
        errorPassword = null
        errorToken = null
        val newPassword = binding.etPassword.text.toString()
        val confirmNewPassword = binding.etRepeatPassword.text.toString()

        errorPassword = viewModel.getPasswordValidationError(newPassword, confirmNewPassword)
        errorToken = viewModel.getTokenValidationError(codes)
        if (errorPassword == null && errorToken == null) {
            resetPassword(view)
        } else if (errorToken != null) {
            showTokenError()
        } else if (errorPassword != null) {
            showPasswordError()
        }
    }

    private fun resetPassword(view: View) {
        val email = args.email
        val token = buildString {
            for (i in codes.indices) {
                append(codes[i].text.toString())
            }
        }
        val newPassword = binding.etPassword.text.toString()
        val confirmNewPassword = binding.etRepeatPassword.text.toString()
        val userPasswordReset = UserPasswordReset(email, token, newPassword, confirmNewPassword)
        viewModel.postResetPassword(userPasswordReset)
        viewModel.isPasswordReset.observe(viewLifecycleOwner) { isPasswordReset ->
            if (isPasswordReset) {
                viewModel.navigateToLoginFragment(view)
            } else {
                showTokenError()
            }
        }
    }

    private fun showPasswordError() {
        binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded_error)
        if (errorPassword == ValidatorPassword.Error.PASSWORD_REPEAT_NO_MATCH) {
            binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded_error)
            binding.tvRepeatedPasswordValidationError.text = errorPassword.toString()
        } else {
            binding.tvPasswordValidationError.text = errorPassword.toString()
        }
    }

    private fun hidePasswordError() {
        binding.tvPasswordValidationError.text = ""
        binding.tvRepeatedPasswordValidationError.text = ""
        binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded)
        binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded)
    }

    private fun showTokenError() {
        Toast.makeText(context, "Wprowadzony kod jest niepoprawny", Toast.LENGTH_SHORT).show()
    }
}