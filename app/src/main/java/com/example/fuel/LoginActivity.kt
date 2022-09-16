package com.example.fuel

import android.graphics.Color
import android.graphics.LinearGradient
import android.graphics.Shader
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.TextView

class LoginActivity : AppCompatActivity() {

    private lateinit var tvWelcomeMsg: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        supportActionBar?.hide()

        tvWelcomeMsg = findViewById(R.id.loginActivity_welcome)
        /*TODO: change gradient values*/
        val shader = LinearGradient(0f, 0f, 0f, tvWelcomeMsg.textSize,
            Color.RED, Color.BLUE, Shader.TileMode.CLAMP)
        tvWelcomeMsg.paint.shader = shader
    }
}