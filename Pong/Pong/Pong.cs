using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class Pong : PhysicsGame
{
    Vector nopeusYlos = new Vector(0, 200);
    Vector nopeusAlas = new Vector(0, -200);

    IntMeter pelaajan1Pisteet;
    IntMeter pelaajan2Pisteet;

    PhysicsObject pallo;
    PhysicsObject maila1;
    PhysicsObject maila2;

    PhysicsObject vasenReuna;
    PhysicsObject oikeaReuna;
    Image olionKuva = LoadImage("creeper");
    Image taustaKuva = LoadImage("nyan");

    public override void Begin()
    {

        LuoKentta();
        AsetaOhjaimet();
        LisaaLaskurit();
        AloitaPeli();


        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Enter, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    void LuoKentta()
    {

        pallo = new PhysicsObject(90.0, 90.0);
        pallo.Image = olionKuva;
        pallo.Color = Color.LimeGreen;
        pallo.X = -200.0;
        pallo.Y = 0.0;
        Add(pallo);

        maila1 = LuoMaila(Level.Left + 30.0, 0.0);
        maila2 = LuoMaila(Level.Right - 30.0, 0.0);

        AddCollisionHandler(pallo, KasittelePallonTormays);

        vasenReuna = Level.CreateLeftBorder();
        vasenReuna.Restitution = 1.0;
        vasenReuna.IsVisible = false;
        oikeaReuna = Level.CreateRightBorder();
        oikeaReuna.Restitution = 1.0;
        oikeaReuna.IsVisible = false;
        PhysicsObject ylaReuna = Level.CreateTopBorder();
        ylaReuna.Restitution = 1.0;
        ylaReuna.IsVisible = false;
        PhysicsObject alaReuna = Level.CreateBottomBorder();
        alaReuna.Restitution = 1.0;
        alaReuna.IsVisible = false;

        pallo.Restitution = 1.5;
        Level.Background.Image = taustaKuva;
        Level.Background.FitToLevel();
        Camera.ZoomToLevel();
    }
    void AloitaPeli()
    {
        Vector impulssi = new Vector(500.0, 500.0);
        pallo.Hit(impulssi);
    }
    PhysicsObject LuoMaila(double x, double y)
    {
        PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0);
        maila.Shape = Shape.Rectangle;
        maila.X = x;
        maila.Y = y;
        maila.Restitution = 5.0;
        Add(maila);
        return maila;
    }
    void AsetaOhjaimet()
    {
        Keyboard.Listen(Key.A, ButtonState.Down, AsetaNopeus, "pelaaja 1: Liikuta mailaa ylös", maila1, nopeusYlos);
        Keyboard.Listen(Key.A, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);
        Keyboard.Listen(Key.Z, ButtonState.Down, AsetaNopeus, "Pelaaja 1:Liikuta mialaa alas", maila1, nopeusAlas);
        Keyboard.Listen(Key.Z, ButtonState.Released, AsetaNopeus, null, maila1, Vector.Zero);

        Keyboard.Listen(Key.Up, ButtonState.Down, AsetaNopeus, "Pelaaja2: Liikuta mailaa ylös", maila2, nopeusYlos);
        Keyboard.Listen(Key.Up, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down, AsetaNopeus, "pelaaja 2: Liikuta mailaa alas", maila2, nopeusAlas);
        Keyboard.Listen(Key.Down, ButtonState.Released, AsetaNopeus, null, maila2, Vector.Zero);

        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

    }


    void AsetaNopeus(PhysicsObject maila, Vector nopeus)
    {
        if ((nopeus.Y < 2) && (maila.Bottom < Level.Bottom))
        {
            maila.Velocity = Vector.Zero;
            return;
        }
        if ((nopeus.Y > 2) && (maila.Top > Level.Top))
        {
            maila.Velocity = Vector.Zero;
            return;
        }
        maila.Velocity = nopeus;
    }
    void LisaaLaskurit()
    {
        pelaajan1Pisteet = LuoPisteLaskuri(Screen.Left + 100.0, Screen.Top - 100.0);
        pelaajan2Pisteet = LuoPisteLaskuri(Screen.Right - 100.0, Screen.Top - 100.0);
    }

    IntMeter LuoPisteLaskuri(double x, double y)
    {
        IntMeter laskuri = new IntMeter(0);
        laskuri.MaxValue = 999;

        Label naytto = new Label();
        naytto.BindTo(laskuri);
        naytto.X = x;
        naytto.Y = y;
        naytto.TextColor = Color.Black;
        naytto.BorderColor = Level.Background.Color;
        naytto.Color = Level.Background.Color;
        Add(naytto);

        return laskuri;
    }
    void KasittelePallonTormays(PhysicsObject pallo, PhysicsObject kohde)
    {
        if (kohde == oikeaReuna)
        {
            pelaajan1Pisteet.Value += 1;
        }
        else if (kohde == vasenReuna)
        {
            pelaajan2Pisteet.Value += 1;
        }

   }
}       