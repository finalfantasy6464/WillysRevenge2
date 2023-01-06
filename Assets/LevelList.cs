using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    public string[] levelNames = {
        "Humble Beginnings",
        "That's the Key",
        "Circular Thinking",
        "The Big Squeeze",
        "Meet Running Roger",
        "One Way Street",
        "Thought Spiral",
        "Meet Bobbing Barry",
        "A-Maze-Ment",
        "Block Party",
        "Commitment Issues",
        "Self Reflection",
        "One Step Forward",
        "Closing in",
        "Roll of the Dice",
        "Illusion of Choice",
        "No Turning Back",
        "Two Wrongs",
        "A Game Of Two Halves",
        "Two Steps Back",
        "Uneven Ground",
        "Going In Circles",
        "Looping Statement",
        "Meet Yielding Yazmin",
        "Roadblock",
        "Snake In The Headlights",
        "A Little Of Everything",
        "Playing Chicken",
        "Abrasive Affection",
        "Go For A Whirl",
        "Shifting Safety",
        "Hot Timing",
        "Infernal Labyrinth",
        "The Sea Of Fire",
        "Pick Your Moment",
        "Meet Gyroscopic Gerry",
        "The Centrifuge",
        "Key, Cross, Goal",
        "A Square Peg",
        "Seventh Circle",
        "Sight for Saw eyes",
        "Rusty Infiltration",
        "Show Me A Sine",
        "Shocking Surprise",
        "Chase Sequence",
        "Machine Input",
        "Conveyance",
        "Processing Plant",
        "Distribution Centre",
        "The Refuse Chute",
        "The Hidden Base",
        "Dancing Betwixt Bullets",
        "The Haywire Cannon",
        "Corridor Cascade",
        "Bullet Barrage",
        "Periodical Punishment",
        "Tight Spaces",
        "Meaningful Motions",
        "Procedural Difficulty",
        "The Cave Escape",
        "Meet Pursuant Peter",
        "The Slowest Door",
        "Action And Reaction",
        "Multipath Maze",
        "Thread The Needle",
        "Combination Lock",
        "Unending Pursuit",
        "Trackside Hazards",
        "Labyrinthine Languish",
        "The Ineffective Chaser",
        "Carried On Clouds",
        "Dodging Detritus",
        "Tense Travel",
        "Temperamental Flooring",
        "Synchronised Shockers",
        "Viney Villains",
        "Look Both Ways",
        "Cyclical Cloud Surfing",
        "The Autoscroller",
        "Congestion In The Clouds",
        "Moon Base Alpha",
        "Clear The Chasms",
        "Gauntlet For The Gold",
        "Whirl of a time",
        "Meet Slinging Simon",
        "Frontal Assault",
        "That Sinking Feeling",
        "Going For A Spin",
        "Beaming With Joy",
        "The Desperate Retreat",
        "A Study On: Toyblock Forest",
        "A Study On: Icicle Palace",
        "A Study On: Shifting Desert",
        "A Study On: Sweltering Volcano",
        "A Study On: Automated Factory",
        "A Study On: Crystal Caverns",
        "A Study On: Jade Ruins",
        "A Study On: Clouded Sky",
        "A Study On: Barren Moon",
        "The Office Of The Boss",
    };

    public int[] parTimes =
    {
        12, 19, 16, 10, 11, 26, 30, 11, 25, 15,
        25, 23, 23, 23, 34, 33, 32, 39, 29, 16,
        18,  9, 23, 16, 33, 19, 47, 51, 50, 34,
         8, 18, 37, 30, 32, 37, 38, 55, 29, 19,
        19, 33, 30, 38, 17, 34, 30, 30, 28, 10,
        38, 54, 15, 11, 30, 28, 36, 43, 24, 34,
        37, 55, 53, 77, 21, 55, 34, 19, 50, 63,
        44, 31, 28,  8, 27, 30, 44, 31, 43, 29,
        55, 43, 12, 63, 35, 68, 71, 60, 61, 35,
        40, 10, 26, 42, 49, 38, 29, 37, 31, 90,
    };
}