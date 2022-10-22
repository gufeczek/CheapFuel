package com.example.fuel.ui.login

import android.content.Intent
import android.graphics.Color
import android.graphics.LinearGradient
import android.graphics.Shader
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.example.fuel.R
import com.example.fuel.ui.login.fragment.SetPasswordFragment
import com.example.fuel.ui.login.fragment.SetRegisterMethodFragment

class LoginActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        supportActionBar?.hide()

        supportFragmentManager
            .beginTransaction()
            .replace(R.id.frameLayout2, SetRegisterMethodFragment())
            .commit()

    }
}