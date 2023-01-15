package com.example.fuel.ui.fragment.login

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentResetPasswordBinding
import com.example.fuel.model.Email
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.validation.ValidatorEmail
import com.example.fuel.viewmodel.UserLoginViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class ResetPasswordFragment : Fragment() {

    private var _binding: FragmentResetPasswordBinding? = null
    private val binding get() = _binding!!
    private var error: ValidatorEmail.Error? = null
    private lateinit var viewModel: UserLoginViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentResetPasswordBinding.inflate(inflater, container, false)
        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserLoginViewModel::class.java]

        binding.clMain.setOnClickListener { view -> view.hideKeyboard() }
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
        binding.btnNextPage.setOnClickListener(btnGoToNextPage)

        return binding.root
    }

    override fun onDestroy() {
        super.onDestroy()
        _binding = null
    }

    private val btnGoToNextPage = View.OnClickListener { view ->
        val email = binding.etEmail.text.toString()
        error = viewModel.getEmailValidationError(email)
        if (error == null) {
            viewModel.getPasswordResetToken(Email(email))
            viewModel.isTokenGenerated.observe(viewLifecycleOwner) { isTokenGenerated ->
                if (isTokenGenerated) {
                    //val action = ResetPasswordFragmentDirections.actionResetPasswordToResetPasswordCodeFragment(email)
                    //viewModel.navigateToResetPasswordCodeFragment(view, action)
                }
            }
        } else {
            showError()
            setErrorTracking()
        }
    }

    private fun showError() {
        binding.tilEmail.setBackgroundResource(R.drawable.bg_rounded_error)
        binding.tvEmailError.text = error.toString()
    }

    private fun hideError() {
        binding.tilEmail.setBackgroundResource(R.drawable.bg_rounded)
        binding.tvEmailError.text = ""
    }

    private fun setErrorTracking() {
        binding.etEmail.afterTextChanged { email ->
            if (viewModel.getEmailValidationError(email) == null) {
                hideError()
                stopErrorTracking()
            }
        }
    }

    private fun stopErrorTracking() {
        binding.etEmail.afterTextChanged {}
    }
}