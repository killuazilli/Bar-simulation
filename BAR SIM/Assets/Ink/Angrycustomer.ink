VAR mood = -2
VAR listeningScore = 0
VAR drink_choice = ""

-> start


=== start

I've been standing here trying to get somebody to help me, and now I've checked my banking app and it looks like I've been charged twice for the same order.

* [You've been waiting for help, and seeing another charge has made it feel like nobody is taking the problem seriously. Can you show me what happened?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Exactly. That's what I've been trying to explain.
    -> previous_staff

* [Let me check the receipt and payment terminal and see what happened.]
    ~ mood = mood + 1
    Fine. At least you're actually looking at it.
    -> previous_staff

* [Sometimes duplicate charges are only temporary, so it may disappear on its own.]
    ~ mood = mood - 1
    That's exactly the kind of answer I've already been given.
    -> previous_staff


=== previous_staff

I already showed another member of staff. He barely looked at my phone and just told me to calm down. That made me even angrier.

* [So the payment problem is frustrating, but being brushed off is what really pushed this over the edge?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yes. Exactly. I don't want to be treated like I'm causing trouble just because I'm asking what happened.
    -> calm_down

* [I'm sure he didn't mean to dismiss you.]
    ~ mood = mood - 1
    You weren't there. That's exactly how it felt.
    -> calm_down

* [I can ask a supervisor to look at the payment with me.]
    ~ mood = mood + 1
    Good. That's at least something practical.
    -> calm_down


=== calm_down

And please don't tell me to calm down. People keep saying that instead of actually listening to what I'm telling them.

* [You're not asking me to calm you down. You want someone to understand the problem and deal with it properly.]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yes. That's exactly what I want.
    -> service_moment

* [I understand you're angry, but raising your voice won't make the payment issue easier to solve.]
    I know I'm angry. I'm angry because nobody has dealt with it.
    -> service_moment

* [Let's just focus on the payment so we can get this sorted.]
    Fine. That's what I've been trying to do.
    -> service_moment


=== service_moment

I've been here long enough already. I was going to order another whiskey while this gets sorted, but honestly I'm too annoyed to even decide what I want. # DRINK_CHOICE

-> evaluate_service


=== evaluate_service

{ drink_choice == "water":
    ~ mood = mood + 1
    Water's probably a better idea right now. Fine.
    -> desired_outcome
}

{ drink_choice == "soft_drink":
    A soft drink is fine. I mostly just want this payment issue sorted.
    -> desired_outcome
}

{ drink_choice == "alcohol":
    ~ mood = mood - 1
    That's what I originally wanted, but another drink isn't really going to fix why I'm annoyed.
    -> desired_outcome
}

{ drink_choice == "listen":
    ~ mood = mood + 1
    Fine. Just let me explain exactly what I'm seeing on my account.
    -> desired_outcome
}

I don't really care about the drink right now.

-> desired_outcome


=== desired_outcome

What I actually want is simple. I need to know whether I've really been charged twice and what you're going to do if I have.

* [You want a clear answer about the charge and a clear next step instead of being passed between people. Have I understood that correctly?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yes. That's all I've been asking for.
    -> escalation

* [I'll get the supervisor so they can check the transaction properly.]
    ~ mood = mood + 1
    Good. That's useful.
    -> escalation

* [I can't control what your bank shows, so there's only so much I can do.]
    ~ mood = mood - 1
    And there it is again. Somebody telling me why it isn't their problem.
    -> escalation


=== escalation

Honestly, if somebody had just explained what was happening at the beginning, I probably wouldn't be this angry. Now I feel like I'm the problem because I've had to keep asking.

* [It sounds like the uncertainty was frustrating, but feeling ignored made the situation much worse. Is that fair?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yeah. That's fair. The money matters, obviously, but being ignored is what really got to me.
    -> resolution

* [At least we're dealing with it now.]
    I suppose. I just wish it hadn't taken this long.
    -> resolution

* [You have been quite loud, so I can understand why staff thought the situation was escalating.]
    ~ mood = mood - 1
    Right. So now we're back to this being my fault.
    -> resolution


=== resolution

Alright. I'm still annoyed, but at least I feel like you actually understood what I was trying to say.

If someone can check the transaction properly and explain what happens next, that's all I need.

* [Finish interaction]
    -> END