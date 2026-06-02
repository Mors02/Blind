EXTERNAL PlaySound(SoundId)
EXTERNAL CompleteAction(objectId, actionId)
EXTERNAL UpdateInventory(objectId, added)
//LIST itemStateList = POSSESSED, NOT_POSSESSED
//LIST interactableStateList = CLOSE, OPEN
VAR drankBottle = false
//VAR keyState = itemStateList.NOT_POSSESSED
//VAR doorState = interactableStateList.CLOSE
VAR keyState = "NOT_POSSESSED"
VAR doorState = "CLOSE"
VAR lampState = "OFF"
VAR skull1State = "NOT_POSSESSED"
VAR skull2State = "NOT_POSSESSED"
VAR skull3State = "NOT_POSSESSED"
VAR skull4State = "NOT_POSSESSED"

//->wardrobe

=== stoneWall ===
A cold wall stands in you way, smooth as a tomb.
Nothing seems to pass through here.
-> END

=== door ===
{doorState:
- "CLOSE": -> door.close
- "OPEN": -> door.open
- else: -> END
}
=open
The door is waiting for you to pass.
+ [- Close it.]
    The wooden door is closed shut. You rotate the key and lock it.
    ~ doorState = "CLOSE"
    ~ CompleteAction("door", "close")
    -> door
+ [- Go away.]
    ->END
=close
A wooden door, closed. The handle is slightly hot, like someone touched recently.
A feint smell of chemicals pass through the door.
-> doorOptions

=== doorOptions ===
+ [- Define the smell]
    It's definetely gasoline. The whole door is covered in it.
    -> doorOptions
+ [?- It wasn't here before]
    You're right, the door wasn't here last time you were here.
    -> doorOptions
+ [- Open it.]
    {keyState == "POSSESSED":
        The key rotates and the lock clicks. The door opens.
        ~ doorState = "OPEN"
        ~ CompleteAction("door", "open")
        -> door
    - else:
        It's locked.
        -> doorOptions
    }
* [- Go away]
    ->END
    
=== floor ===
A squeaky clean marble floor.
-> END

=== fridge ===
Your average metallic fridge.
+ [- Open it]
    Cold air touches your face. It smells like rotten eggs.
    ->openFridge
+ [- Go away]
    -> END

=== openFridge ===
{drankBottle:
    The empty bottle looks at you from the tray.
  - else:
    You find a glass bottle in the lower tray, you don't remember putting one there.
}

{not drankBottle: 
+ [- Drink it]
    The cap is loose, you pop it. Your lips touch the cold glass and a metallic taste feels your mouth. It's full of blood.
    ** [- Keep drinking]
        Everything in your head knows that you shouldn't do it, but it's too late. The cold blood pours down your throat. You can barely hold the vomit.
        ~ drankBottle = true    
        ->openFridge
    ++ [- Put it back]
        You put the bottle back into the lower tray.
        ->openFridge
    ++ [?- Why is there blood in my fridge]
        Are you asking me?
        -> openFridge
* [- Leave it be]
 You close the fridge.
 -> DONE
 - else:
    * [- Take the empty bottle]
    It could come in handy.
    ->END
}

=== clock ===
Your clock.
->END

=== wardrobe ===
As always, your wardrobe is full of useless crap.
{keyState:
- "POSSESSED":
    -> wardrobe.withKey
- "NOT_POSSESSED": 
    -> wardrobe.withoutKey
- else:
    -> END
}

= withKey
    Better leave this here.
    //-> door
    -> END
= withoutKey
+ [- Open the wardrobe]
    ~ PlaySound("CabinetWoodOpen")
    -> wardrobeOptions
+ [- Go away]
    -> END
=== wardrobeOptions ===
    You see used clothes, an old hockey stick, some boxes and a small drawer.
    ** [- Look inside the boxes]
        A pile of underwater and some socks.
        -> wardrobeOptions
    ** [- Look inside the drawer]
        ~ PlaySound("DrawerMediumOpen")
         You found a key, looks like the duplicate you made for your entrance.
        *** [- Take the key]
        You took the key and put it in your pocket.
        ~ keyState = "POSSESSED"
        ~ UpdateInventory("key", true)
        ~ PlaySound("DrawerMediumClose")
        -> wardrobeOptions
    ++ [- Close the wardrobe]
        ~ PlaySound("CabinetWoodClose")
        -> wardrobe
    //- -> wardrobe


=== chair ===
Your chair.
-> END

=== lamp ===
A simple lamp.
    {lampState:
    - "ON":
        -> lamp.on
    - "OFF": 
        -> lamp.off
    - else:
        -> END
    }
    
= on
    You feel the heat radiating from the lamp. You really are blind.
    + [- Turn off.]
        ~ lampState = "OFF"
        ~ PlaySound("LampDeskTurnoff")
        -> lamp
    + [- Go away.]
        -> END
= off
    It stands on the bedside table.
    + [- Turn on.]
        ~ lampState = "ON"
        ~ PlaySound("LampDeskTurnon")
        -> lamp
    + [- Go away.]
        -> END


=== kitchenWorktop ===
Your kitchen.
-> END

=== bed ===
Your bed.
-> END

=== bedsideTable ===
Your bedside table.
-> END
     
=== table ===
Your table.
-> END

=== carpet ===
Your carpet.
-> END

=== key ===
Your key.  It opens the front door.
+ [- Check it closer.]
    You let the metal teeth on you fingers. This really seems the same key you always had.
    -> key
+ {keyState == "NOT_POSSESSED"} [- Take it.]
    You put it in your pocket.
    ~ keyState = "POSSESSED"
    ~ UpdateInventory("key", true)
    -> END

+ {keyState == "POSSESSED"} [- Put away.]
    -> END
    
=== elevator ===
Let's get the fuck out of here.
+ [- Press button.]
    ~ CompleteAction("elevator", "down")
    -> END
+ [- Go away.]
-> END

=== grate ===
An heavy metal grate. The rust sticks to your hand, covering them in a fine dust.
-> END

=== altar ===
Feels cold on the touch. The air around it is rotten, like something died near it long time ago.
There are three pools of liquid on top of it, two of them contains what looks like a elongated skull.
+ {skull1 == "POSSESSED"} [- Put the small skull inside.]
    You put the small skull inside, a loud scream may have alerted something of your presence.
    -> END
+ {skull2 == "POSSESSED"} [- Put the heavy skull inside.]
    You put the heavy skull inside, a loud scream may have alerted something of your presence.
    -> END
+ {skull3 == "POSSESSED"} [- Put the cold skull inside.]
    You put the cold skull inside, a loud scream may have alerted something of your presence.
    -> END
+ {skull4 == "POSSESSED"} [- Put the strange skull inside.]
    You put the strange skull inside and weird whirring comes from the altar.
    An image of white beings running gets impressed on your retina, a flow of information so large that feels like a spike in your head.
    The metal grate behind seems unlocked.
    -> END
+ [- Go away]
-> END

=== skull1 ===
A small metallic skull with an oblung shape.
It produces a repetitive sound.
+ [- Check it closer.]
    The teeth are missing and the eye hole are too big to be human. It belonged to something else.
    -> skull1
+ {skull1State == "NOT_POSSESSED"} [- Take it.]
    You put it in your pocket.
    ~ skull1State = "POSSESSED"
    ~ UpdateInventory("skull1", true)
    -> END

+ {skull1State == "POSSESSED"} [- Put away.]
    -> END
    
=== skull2 ===
An heavy metallic skull with an oblung shape.
It produces a repetitive sound.
+ [- Check it closer.]
    The teeth are missing and the eye hole are too big to be human. It belonged to something else.
    -> skull2
+ {skull2State == "NOT_POSSESSED"} [- Take it.]
    You put it in your pocket.
    ~ skull2State = "POSSESSED"
    ~ UpdateInventory("skull2", true)
    -> END

+ {skull2State == "POSSESSED"} [- Put away.]
    -> END
    
=== skull3 ===
A cold metallic skull with an oblung shape.
It produces a repetitive sound.
+ [- Check it closer.]
    The teeth are missing and the eye hole are too big to be human. It belonged to something else.
    -> skull3
+ {skull3State == "NOT_POSSESSED"} [- Take it.]
    You put it in your pocket.
    ~ skull3State = "POSSESSED"
    ~ UpdateInventory("skull3", true)
    -> END

+ {skull3State == "POSSESSED"} [- Put away.]
    -> END
    
=== skull4 ===
A strange metallic skull with an oblung shape.
It produces a repetitive sound.
+ [- Check it closer.]
    The teeth are missing and the eye hole are too big to be human. It belonged to something else.
    -> skull4
+ {skull4State == "NOT_POSSESSED"} [- Take it.]
    You put it in your pocket.
    ~ skull4State = "POSSESSED"
    ~ UpdateInventory("skull4State", true)
    -> END

+ {skull4State == "POSSESSED"} [- Put away.]
    -> END
     