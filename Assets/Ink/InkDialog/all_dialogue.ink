//->fridge

=== stoneWall ===
A cold wall stands in you way, smooth as a tomb.
Nothing seems to pass through here.
-> END

=== door ===
A wooden door, closed. The handle is slightly hot, like someone touched recently.
A feint smell of chemicals pass through the door.
-> doorOptions

=== doorOptions ===
+ - Define the smell
    It's definetely gasoline. The whole door is covered in it.
    -> doorOptions
+ ?- It wasn't here before
    You're right, the door wasn't here last time you were here.
    -> doorOptions
* [- Go away]
    ->END
    
=== floor ===
A squeaky clean marble floor.
-> END

=== fridge ===
Your average metallic fridge.
+ - Open it
    Cold air touches your face. It smells like rotten eggs.
    ->openFridge
+ - Go away
    -> END

VAR drankBottle = false
=== openFridge ===
{drankBottle:
    The empty bottle looks at you from the tray.
  - else:
    You find a glass bottle in the lower tray, you don't remember putting one there.
}

{not drankBottle: 
+ - Drink it
    The cap is loose, you pop it. Your lips touch the cold glass and a metallic taste feels your mouth. It's full of blood.
    ** - Keep drinking
        Everything in your head knows that you shouldn't do it, but it's too late. The cold blood pours down your throat. You can barely hold the vomit.
        ~ drankBottle = true    
        ->openFridge
    ++ - Put it back
        You put the bottle back into the lower tray.
        ->openFridge
    ++ ?- Why is there blood in my fridge
        Are you asking me?
        -> openFridge
* - Leave it be
 You close the fridge.
 -> DONE
 - else:
    * - Take the empty bottle
    It could come in handy.
    ->END
}

=== clock ===
Your clock.
->END

=== wardrobe ===
Your wardrobe.
-> END

=== chair ===
Your chair.
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
     
     
     