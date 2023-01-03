package com.example.fuel.ui.fragment.register

import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import androidx.navigation.fragment.navArgs
import com.example.fuel.R
import com.example.fuel.databinding.FragmentPasswordBinding
import com.example.fuel.model.account.UserRegistration
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.validation.ValidatorPassword
import com.example.fuel.utils.validation.Validator.Companion.isAtLeastOneDigit
import com.example.fuel.utils.validation.Validator.Companion.isAtLeastOneUpperCase
import com.example.fuel.viewmodel.UserRegistrationViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class PasswordFragment : Fragment(R.layout.fragment_password) {

    private var _binding: FragmentPasswordBinding? = null
    private val binding get() = _binding!!
    private var error: ValidatorPassword.Error? = null
    private lateinit var viewModel: UserRegistrationViewModel
    private val args: PasswordFragmentArgs by navArgs()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentPasswordBinding.inflate(inflater, container, false)
        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserRegistrationViewModel::class.java]

        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
        binding.clMain.setOnClickListener { view -> view.hideKeyboard() }

        binding.etPassword.afterTextChanged { editable ->
            passwordCheckmarks(editable)
        }
        binding.btnNextPage.setOnClickListener(btnGoToNextPage)

        return binding.root
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        (activity as AppCompatActivity).supportActionBar?.hide()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private val btnGoToNextPage = View.OnClickListener { view ->
        hideError()

        val password = binding.etPassword.text.toString()
        val repeatedPassword = binding.etRepeatPassword.text.toString()
        error = viewModel.getPasswordValidationError(password, repeatedPassword)

        if (error == null) {
            viewModel.user.value?.password = password
            viewModel.user.value?.confirmPassword = password
            registerUser(password)
            viewModel.response.observe(viewLifecycleOwner) { response ->
                if (response.isSuccessful) {
                    viewModel.navigateToLoginFragment(view)
                }
            }
        } else {
            showError()
        }
    }

    private fun passwordCheckmarks(text: String) {
        val greenTick = R.drawable.ic_tick_green_32dp
        val redCancel = R.drawable.ic_cancel_red_47dp

        if (text.length >= 8) {
            binding.imageAtLeast8Characters.setImageResource(greenTick)
        } else {
            binding.imageAtLeast8Characters.setImageResource(redCancel)
        }

        if (isAtLeastOneUpperCase(text)) {
            binding.imageAtLeastOneCapitalLetter.setImageResource(greenTick)
        } else {
            binding.imageAtLeastOneCapitalLetter.setImageResource(redCancel)
        }

        if (isAtLeastOneDigit(text)) {
            binding.imageAtLeastOneDigit.setImageResource(greenTick)
        } else {
            binding.imageAtLeastOneDigit.setImageResource(redCancel)
        }
    }

    private fun showError() {
        binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded_error)
        if (error == ValidatorPassword.Error.PASSWORD_REPEAT_NO_MATCH) {
            binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded_error)
            binding.tvRepeatedPasswordValidationError.text = error.toString()
        } else {
            binding.tvPasswordValidationError.text = error.toString()
        }
    }

    private fun hideError() {
        binding.tvPasswordValidationError.text = ""
        binding.tvRepeatedPasswordValidationError.text = ""
        binding.tilPassword.setBackgroundResource(R.drawable.bg_rounded)
        binding.tilRepeatPassword.setBackgroundResource(R.drawable.bg_rounded)
    }

    private fun registerUser(password: String) {
        val username = args.username
        val email = args.email
        val user = UserRegistration(username, email, password, password)
        viewModel.postRegister(user)
    }
}