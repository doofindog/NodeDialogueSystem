using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor;
using UnityEngine;

public class NodeTextField : NodeComponent
{
   private string _label;
   private string _text;

   private Rect _labelRect;
   private Rect _textRect;

   public NodeTextField(Rect rect)
   {
      base.rect = rect;

      Vector2 labelPosition = rect.position;
      Vector2 labelSize = new Vector2()
      {
         x = rect.size.x * 0.3f,
         y = rect.size.y
      };
      _labelRect = new Rect(labelPosition, labelSize);

      Vector2 textPosition = new Vector2()
      {
         x = labelPosition.x + labelSize.x + 10,
         y = rect.position.y
      };
      Vector2 textSize = new Vector2()
      {
         x = rect.size.x * 0.65f,
         y = rect.size.y
      };
      _textRect = new Rect(textPosition, textSize);
   }

   public string Draw(string label,string pText = null)
   {
      if (string.IsNullOrEmpty(pText))
      {
         pText = _text;
      }
      
      GUI.skin.textArea.wordWrap = true;
      GUI.Label(_labelRect,label);
      _text = GUI.TextArea(_textRect, pText);
      GUI.skin.textArea.wordWrap = false;
      
      return _text;
   }
}
