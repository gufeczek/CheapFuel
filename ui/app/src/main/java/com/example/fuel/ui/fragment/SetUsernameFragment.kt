package com.example.fuel.ui.fragment

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
import androidx.navigation.Navigation
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetUsernameBinding
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.extension.TextViewExtension.Companion.removeLinksUnderline
import com.example.fuel.utils.validation.ValidatorUsername
import java.util.*


class SetUsernameFragment : Fragment(R.layout.fragment_set_username) {

    private var _binding: FragmentSetUsernameBinding? = null
    private val binding get() = _binding!!
    private var error: ValidatorUsername.Error? = null


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentSetUsernameBinding.inflate(inflater, container, false)
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
        binding.tvTermsOfUse.movementMethod = LinkMovementMethod.getInstance()
        binding.tvTermsOfUse.removeLinksUnderline()

        binding.btnNextPage.setOnClickListener(btnValidationListener)
        binding.chkTermsOfUse.setOnCheckedChangeListener(chkValidationListener)

        binding.chkTermsOfUse.setOnClickListener{ view -> view.hideKeyboard() }
        binding.clMain.setOnClickListener{ view -> view.hideKeyboard() }

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
    override fun onAttach(context: Context) {
        super.onAttach(context)

        (activity as AppCompatActivity).supportActionBar?.hide()
    }

    private val btnValidationListener = View.OnClickListener {
        val etUsernameText = binding.etUsername.text.toString()
        val validatorUsername = ValidatorUsername(etUsernameText)
        val isValidated = validatorUsername.validate()
        error = validatorUsername.error
        if (isValidated && binding.chkTermsOfUse.isChecked) {
            Navigation.findNavController(binding.root).navigate(R.id.setPasswordFragment)
        }
        else if (!isValidated) {
            binding.tilUsername.setBackgroundResource(R.drawable.bg_rounded_error)
            binding.tvUsernameValidationError.text = error.toString()
            binding.tvUsernameValidationError.visibility = View.VISIBLE
            if (error == ValidatorUsername.Error.ERROR_USERNAME_TOO_SHORT || error == ValidatorUsername.Error.ERROR_USERNAME_TOO_LONG) {
                binding.etUsername.afterTextChanged {
                    if (validatorUsername.validate()) {
                        binding.tilUsername.setBackgroundResource(R.drawable.bg_rounded)
                        binding.tvUsernameValidationError.visibility = View.INVISIBLE
                    }
                }
            }
        }
        else if (!binding.chkTermsOfUse.isChecked) {
            binding.tvTermsOfUseError.visibility = View.VISIBLE
            binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(Color.RED)
        }
    }

    private val chkValidationListener = CompoundButton.OnCheckedChangeListener { _, isChecked ->
        if (isChecked) {
            binding.tvTermsOfUseError.visibility = View.GONE
            binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(resources.getColor(R.color.checkbox, activity?.theme))
            binding.tvTermsOfUse.setLinkTextColor(resources.getColor(R.color.checkbox, activity?.theme))
        }
    }
}