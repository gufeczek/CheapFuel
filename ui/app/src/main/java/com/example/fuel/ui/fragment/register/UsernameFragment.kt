package com.example.fuel.ui.fragment.register

import android.content.Context
import android.content.res.ColorStateList
import android.graphics.Color
import android.os.Bundle
import android.text.method.LinkMovementMethod
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.CompoundButton
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import androidx.navigation.fragment.navArgs
import com.example.fuel.R
import com.example.fuel.databinding.FragmentUsernameBinding
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.extension.TextViewExtension.Companion.removeLinksUnderline
import com.example.fuel.utils.validation.ValidatorUsername
import com.example.fuel.viewmodel.UserRegistrationViewModel
import com.example.fuel.viewmodel.ViewModelFactory


class UsernameFragment : Fragment(R.layout.fragment_username) {

    private var _binding: FragmentUsernameBinding? = null
    private val binding get() = _binding!!
    private var error: ValidatorUsername.Error? = null
    private lateinit var viewModel: UserRegistrationViewModel
    private val args: UsernameFragmentArgs by navArgs()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentUsernameBinding.inflate(inflater, container, false)
        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserRegistrationViewModel::class.java]

        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
        binding.tvTermsOfUse.movementMethod = LinkMovementMethod.getInstance()
        binding.tvTermsOfUse.removeLinksUnderline()

        binding.btnNextPage.setOnClickListener(btnGoToNextPage)
        binding.chkTermsOfUse.setOnCheckedChangeListener(chkTermsOfUseValidation)

        binding.chkTermsOfUse.setOnClickListener{ view -> view.hideKeyboard() }
        binding.clMain.setOnClickListener{ view -> view.hideKeyboard() }

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
        val username = binding.etUsername.text.toString()
        val email = args.email
        error = viewModel.getUsernameValidationError(username)
        if (error == null && isTermsOfUseChecked()) {
            val action = UsernameFragmentDirections.navigateToPasswordFragment(username, email)
            viewModel.navigateToPasswordFragment(view, action)
        } else if (error != null) {
            showUsernameValidationError()
            setUsernameErrorTracking()
        } else if (!isTermsOfUseChecked()) {
            showTermsOfUseError()
        }
    }

    private val chkTermsOfUseValidation= CompoundButton.OnCheckedChangeListener { _, isChecked ->
        if (isChecked) {
            hideTermsOfUseError()
        }
    }

    private fun isTermsOfUseChecked(): Boolean {
        return binding.chkTermsOfUse.isChecked
    }

    private fun showUsernameValidationError() {
        binding.tilUsername.setBackgroundResource(R.drawable.bg_rounded_error)
        binding.tvUsernameValidationError.text = error.toString()
        binding.tvUsernameValidationError.visibility = View.VISIBLE
    }

    private fun hideUsernameValidationError() {
        binding.tvUsernameValidationError.visibility = View.INVISIBLE
        binding.tilUsername.setBackgroundResource(R.drawable.bg_rounded)
    }

    private fun showTermsOfUseError() {
        binding.tvTermsOfUseError.visibility = View.VISIBLE
        binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(Color.RED)
    }

    private fun hideTermsOfUseError() {
        binding.tvTermsOfUseError.visibility = View.GONE
        binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(resources.getColor(R.color.checkbox, activity?.theme))
    }

    private fun setUsernameErrorTracking() {
        binding.etUsername.afterTextChanged { username ->
            if (viewModel.getUsernameValidationError(username) == null) {
                hideUsernameValidationError()
                stopUsernameErrorTracking()
            }
        }
    }

    private fun stopUsernameErrorTracking() {
        binding.etUsername.afterTextChanged {}
    }
}