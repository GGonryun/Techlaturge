﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public WordEventHandler WordCreated;
    public ScoreEventHandler WordCompleted;
    [SerializeField] float frequency = .01f;
    [SerializeField] int maximumWords = 100;
    WordDictionary dictionary;

    protected override void Awake()
    {
        base.Awake();
        dictionary = new WordDictionary(frequency, maximumWords);
    }

    void Start()
    {
        CreateWord();
    }

    void OnEnable()
    {
        Keyboard.Instance.Terminate += CompleteWord;
    }

    void OnDisable()
    {
        Keyboard.Instance.Terminate -= CompleteWord;
    }

    void CompleteWord(object sender, KeyPressedEventArgs e)
    {
        SubmitWord();
        CreateWord();
    }

    void SubmitWord()
    {
        KeyValuePair<string, string> currentWord = WordBuilder.Instance.ClearWord();
        Entry entry = new Entry(currentWord.Key, currentWord.Value, LetterReader.Instance.Score);
        WordCompleted?.Invoke(this, new EntryEventArgs(entry));
    }

    void CreateWord()
    {
        Word word = WordBuilder.Instance.CreateWord(dictionary.GetRandom());
        WordCreated?.Invoke(this, new WordEventArgs(word));
    }
}