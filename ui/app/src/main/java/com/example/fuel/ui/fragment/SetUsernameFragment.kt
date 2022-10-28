package com.example.fuel.ui.fragment

import android.content.res.ColorStateList
import android.graphics.Color
import android.os.Bundle
import android.text.method.LinkMovementMethod
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetUsernameBinding
import com.example.fuel.ui.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.ui.utils.extension.TextViewExtension.Companion.removeLinksUnderline
import java.util.*
import kotlin.jvm.optionals.toSet

class SetUsernameFragment : Fragment() {

    private lateinit var binding: FragmentSetUsernameBinding

    private enum class ValidationErrors {
        ERROR_USERNAME_TOO_SHORT,
        ERROR_USERNAME_TOO_LONG
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentSetUsernameBinding.inflate(inflater, container, false)
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
        binding.tvTermsOfUse.movementMethod = LinkMovementMethod.getInstance()
        binding.tvTermsOfUse.removeLinksUnderline()

        binding.btnNextPage.setOnClickListener {
            val (isValidationPassed: Boolean, _msg: Optional<ValidationErrors>) = validateUsername(binding.etUsername.text.toString())
            if (!isValidationPassed && _msg.isPresent) {
                val msg: ValidationErrors = _msg.get()
                binding.tilUsername.setBackgroundResource(R.drawable.bg_rounded_error)
                binding.tvUsernameValidationError.text = msg.toString()
                binding.tvUsernameValidationError.visibility = View.VISIBLE
                if (msg == ValidationErrors.ERROR_USERNAME_TOO_SHORT || msg == ValidationErrors.ERROR_USERNAME_TOO_LONG) {
                    binding.etUsername.afterTextChanged { editable ->
                        if (validateUsername(editable).first) {
                            binding.tilUsername.setBackgroundResource(R.drawable.bg_rounded)
                            binding.tvUsernameValidationError.visibility = View.INVISIBLE
                        }
                    }
                }
            }
            else if (!binding.chkTermsOfUse.isChecked) {
                binding.tvTermsOfUseError.visibility = View.VISIBLE
                binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(Color.RED)
                binding.tvTermsOfUse.setLinkTextColor(Color.RED)
            }
        }
        binding.chkTermsOfUse.setOnCheckedChangeListener { _, isChecked ->
            if (isChecked) {
                binding.tvTermsOfUseError.visibility = View.GONE
                binding.chkTermsOfUse.buttonTintList = ColorStateList.valueOf(resources.getColor(R.color.checkbox, activity?.theme))
                binding.tvTermsOfUse.setLinkTextColor(resources.getColor(R.color.checkbox, activity?.theme))
            }
        }

        return binding.root
    }

    private fun validateUsername(username: String): Pair<Boolean, Optional<ValidationErrors>>{
        if (username.length < 3) {
            return Pair(false, Optional.of(ValidationErrors.ERROR_USERNAME_TOO_SHORT))
        } else if (username.length > 32) {
            return Pair(false, Optional.of(ValidationErrors.ERROR_USERNAME_TOO_LONG))
        }
        return Pair(true, Optional.empty<ValidationErrors>())
    }
}