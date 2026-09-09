VAR mood = -2
VAR listeningScore = 0
VAR drink_choice = ""

-> start


=== start

I nearly went home instead of coming in. I just couldn't face sitting there by myself tonight.

* [Sounds like being alone felt too much tonight. What happened?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yeah... I think that's exactly it.
    -> loss_reveal

* [Take your time. There's no rush.]
    Thanks. I appreciate that.
    -> loss_reveal

* [Sometimes getting out of the house can help.]
    Maybe. That's partly why I came here.
    -> loss_reveal


=== loss_reveal

My aunt died three days ago. She was basically a second mum to me.

* [She meant a lot to you. What do you miss most about her?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Her calls. She used to call me every Sunday, even if we had nothing to talk about.
    -> unwanted_help

* [I'm really sorry for your loss.]
    Thank you. I know people mean well when they say that.
    -> unwanted_help

* [At least you still have the rest of your family around you.]
    ~ mood = mood - 1
    I know... but it doesn't really make losing her hurt less.
    -> unwanted_help


=== unwanted_help

Everyone keeps telling me to stay positive or that she's in a better place. I know they're trying to help, but sometimes I just want to be sad without somebody trying to fix it.

* [You don't need someone to fix it. You just want someone to hear you.]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yes. Exactly. That's what I've been trying to say.
    -> service_moment

* [Maybe you should tell your family you need some space.]
    ~ mood = mood + 1
    Maybe I should. I don't think they realise how overwhelmed I feel.
    -> service_moment

* [That sounds exhausting.]
    Yeah. It really is.
    -> service_moment


=== service_moment

I haven't really looked after myself today either. I barely ate, and I don't think I've had much to drink. I came in planning to order something, but now I'm not sure what I want. # DRINK_CHOICE

-> evaluate_service


=== evaluate_service

{ drink_choice == "water":
    ~ mood = mood + 1
    Water actually sounds good. I probably need that.
    -> family_pressure
}

{ drink_choice == "soft_drink":
    Yeah, something soft is fine.
    -> family_pressure
}

{ drink_choice == "alcohol":
    I thought I wanted alcohol when I came in, but I'm not so sure anymore.
    -> family_pressure
}

{ drink_choice == "listen":
    ~ mood = mood + 1
    Actually... could we just keep talking for another minute?
    -> family_pressure
}

Give me another minute. I'm still deciding.

-> family_pressure


=== family_pressure

My mum is struggling too, so everyone keeps telling me I need to be strong for her. Sometimes it feels like I'm carrying her grief as well as mine.

* [You're dealing with your own grief while feeling responsible for hers too. What do you need right now?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Honestly? I think I just need a break from pretending I'm okay.
    -> self_judgement

* [Maybe your sister could support your mum tonight so you can take a break.]
    ~ mood = mood + 1
    She probably could. I've been trying to do everything myself.
    -> self_judgement

* [That's a lot for one person to carry.]
    Yeah. It really is.
    -> self_judgement


=== self_judgement

I still feel guilty, though. Everyone else is grieving too. Part of me thinks I should be handling this better.

* [It sounds like you're judging yourself for not grieving the way you think you should.]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yeah... I think I am putting most of that pressure on myself.
    -> ending

* [Three days is still very recent.]
    ~ mood = mood + 1
    That's true. I suppose I'm expecting a lot from myself.
    -> ending

* [Try not to dwell on it too much.]
    ~ mood = mood - 1
    I don't think it's that easy.
    -> ending


=== ending

I'm still sad. I know one conversation isn't going to change what happened, but talking about her tonight helped.

Thanks for hearing me out.

* [Finish interaction]
    -> END