package com.example.fuel

import android.graphics.Color
import android.graphics.LinearGradient
import android.graphics.Shader
import android.os.Bundle
import android.widget.EditText
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class LoginActivity : AppCompatActivity() {

    private lateinit var tvWelcomeMsg: TextView
    private lateinit var etEmail: EditText

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        supportActionBar?.hide()

        tvWelcomeMsg = findViewById(R.id.loginActivity_tv_welcome)
        etEmail = findViewById(R.id.loginActivity_et_email)

        /*TODO: change gradient values*/
        val shader = LinearGradient(
            0f, 0f, 0f, tvWelcomeMsg.textSize,
            Color.RED, Color.BLUE, Shader.TileMode.CLAMP
        )
        tvWelcomeMsg.paint.shader = shader

        CornerDrawable.roundCorners(etEmail, 20f)


    }
}