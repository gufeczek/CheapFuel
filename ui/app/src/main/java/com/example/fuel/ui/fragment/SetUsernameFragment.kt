package com.example.fuel.ui.fragment

import android.content.res.ColorStateList
import android.graphics.Color
import android.os.Bundle
import android.text.method.LinkMovementMethod
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.CompoundButton
import androidx.fragment.app.Fragment
import androidx.navigation.Navigation
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetUsernameBinding
import com.example.fuel.ui.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.ui.utils.extension.TextViewExtension.Companion.removeLinksUnderline
import java.util.*


class SetUsernameFragment : Fragment(R.layout.fragment_set_username) {

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

        binding.btnNextPage.setOnClickListener(btnValidationListener)
        binding.chkTermsOfUse.setOnCheckedChangeListener(chkValidationListener)

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

    private val btnValidationListener = View.OnClickListener {
        val (isValidationPassed: Boolean, _msg: Optional<ValidationErrors>) = validateUsername(binding.etUsername.text.toString())
        if (isValidationPassed && binding.chkTermsOfUse.isChecked) {
            Navigation.findNavController(binding.root).navigate(R.id.setPasswordFragment)
        }

        else if (!isValidationPassed && _msg.isPresent) {
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