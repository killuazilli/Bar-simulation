VAR mood = -2
VAR listeningScore = 0
VAR drink_choice = ""

VAR serviceResponse = ""

-> start


=== start

I nearly didn't come in tonight. I stood outside for a few minutes wondering whether I should just go home, but I really didn't want to be there by myself.

* [It sounds like being at home alone felt difficult tonight. What happened?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yeah... I think being there was making everything feel worse.
    -> loss_reveal

* [Well, you're here now. Take your time.]
    Thanks. I appreciate that.
    -> loss_reveal

* [Sometimes getting out of the house can help after a difficult day.]
    Maybe. I think that's partly why I came out.
    -> loss_reveal


=== loss_reveal

My aunt died three days ago. Calling her my aunt almost feels strange because she was basically a second mum to me. Everyone keeps asking if I'm okay, and I honestly don't know what I'm supposed to say.

* [She sounds like she was much more than just an aunt to you. What was she like?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    She really was. She was loud, stubborn, funny... and somehow she always knew when something was wrong with me before I even said anything.
    -> funeral_boundary

* [I'm really sorry for your loss.]
    Thank you. I've heard that a lot this week, but I know people mean well.
    -> funeral_boundary

* [Three days ago? That's very recent.]
    Yeah. Everything still feels a bit unreal.
    -> funeral_boundary


=== funeral_boundary

The funeral arrangements have been exhausting too. Everyone wants to talk about dates, flowers, who is travelling... Honestly, I don't really want to talk about any of that right now.

* [Of course. We don't have to talk about it.]
    ~ mood = mood + 1
    Thanks. I think I just needed someone not to ask me another question about the funeral.
    -> sunday_calls

* [That's fine. Take your time.]
    Thanks. I appreciate that.
    -> sunday_calls

* [Maybe taking a break from the arrangements for a day would help.]
    Maybe. I don't think I can completely avoid it, but I definitely need a break from thinking about it.
    -> sunday_calls


=== sunday_calls

She used to call me every Sunday morning. Sometimes we'd talk for an hour and sometimes for five minutes. Yesterday I picked up my phone because I was going to call her... and then I remembered.

* [Those calls were part of your life with her. It must feel strange having that routine suddenly gone.]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yeah. That's exactly it. It's all the little things that keep catching me off guard.
    -> memories

* [Maybe you could write down some of your memories of her while they're still fresh.]
    ~ mood = mood + 1
    Actually... that's not a bad idea. There are so many ridiculous stories about her that I don't want to forget.
    -> memories

* [Every Sunday?]
    Every single Sunday. She almost never missed one.
    -> memories


=== memories

She made everything feel safe somehow. Whenever something went wrong, I could call her and she'd listen without immediately telling me what I should do. Everyone around me now is trying so hard to make me feel better.

* [So you don't just miss her advice. You miss having someone who would let you talk without trying to fix everything.]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yes. That's exactly it. I don't need somebody to magically make this disappear.
    -> service_moment

* [That sounds really difficult.]
    It is. I know everyone is trying their best.
    -> service_moment

* [People sometimes don't know what to say when someone is grieving.]
    Yeah. I know they're trying to help.
    -> service_moment


=== service_moment

I haven't really been looking after myself either. I barely ate today, I've hardly slept, and I don't think I've had much to drink since this morning. I came in thinking I'd order something, but now I'm not really sure what I want. # DRINK_CHOICE

-> evaluate_service


=== evaluate_service

{ drink_choice == "water":
    ~ mood = mood + 1
    ~ serviceResponse = "Water actually sounds good. I didn't realise how thirsty I was until now."
    -> after_service
}

{ drink_choice == "soft_drink":
    ~ serviceResponse = "Yeah, something soft sounds fine. Something simple is probably best tonight."
    -> after_service
}

{ drink_choice == "alcohol":
    ~ serviceResponse = "I thought alcohol was what I wanted when I walked in. I'm not completely sure that's what I need anymore, though."
    -> after_service
}

{ drink_choice == "listen":
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    ~ serviceResponse = "Actually... could we just keep talking for another minute? I think that's helping more than I expected."
    -> after_service
}

~ serviceResponse = "Give me another minute. I'm still figuring out what I want."
-> after_service

=== after_service

{serviceResponse}

My mum is taking everything really badly. Everyone keeps saying that I need to be strong for her. I understand why, but sometimes it feels like I'm not allowed to be upset because I'm supposed to be looking after everyone else.

* [It sounds like you're carrying your own grief while also feeling responsible for everyone else's. What do you need for yourself right now?]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Honestly? I think I just need permission to stop pretending I'm fine, even if it's only for one night.
    -> sister_decision

* [Maybe somebody else in the family could support your mum for a while so you can take a break.]
    ~ mood = mood + 1
    My sister probably could. I've been trying to do everything myself.
    -> sister_decision

* [That's a lot for one person to carry.]
    Yeah. It really is.
    -> sister_decision


=== sister_decision

My sister actually asked me to stay with her tonight. Part of me wants to because I don't really want to be alone, but another part of me wants my own space. I genuinely don't know what I should do.

* [Maybe you could stay with her tonight and see how you feel tomorrow. It doesn't have to be a decision about the whole week.]
    ~ mood = mood + 1
    That actually sounds manageable. One night doesn't feel as overwhelming as thinking about the whole week.
    -> self_judgement

* [I can see why you're torn.]
    Yeah. Both options feel right and wrong at the same time.
    -> self_judgement

* [You don't have to decide this second.]
    That's true. I suppose I've been treating everything like it needs an answer immediately.
    -> self_judgement


=== self_judgement

Sometimes I feel guilty for even struggling this much. My mum lost her sister, and other people in the family are grieving too. Part of me keeps thinking I should be handling this better.

* [It sounds like you're comparing your grief with everyone else's and judging yourself for feeling it differently.]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yeah... I hadn't really thought about it like that. Nobody has actually told me I'm handling it badly. I think I'm putting that pressure on myself.
    -> final_reflection

* [Grief is difficult for everyone in different ways.]
    Yeah. I suppose it is.
    -> final_reflection

* [You don't have to have everything figured out three days after losing someone.]
    ~ mood = mood + 1
    That's probably true. Three days isn't exactly a long time.
    -> final_reflection


=== final_reflection

You know... when I came in, I thought I needed something that would make me stop feeling sad. But I don't think that's actually possible. Maybe feeling sad isn't the problem.

* [It sounds like you didn't need the sadness taken away. You needed somewhere you could feel it without someone trying to change it.]
    ~ listeningScore = listeningScore + 1
    ~ mood = mood + 1
    Yeah. That's exactly what I needed tonight.
    -> ending

* [I'm glad talking about it helped.]
    Yeah. Me too.
    -> ending

* [I'm glad you decided to come in tonight.]
    Thanks. I think I am too.
    -> ending


=== ending

I'm still sad, and I know one conversation isn't going to change what happened. But talking about my aunt tonight helped me get some of it out.

Thanks for giving me somewhere to sit for a while.

* [Finish interaction]
    -> END